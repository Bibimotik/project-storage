using System.Windows;

using application.Abstraction.Interfaces;
using application.MVVM.Model;
using application.Utilities;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Microsoft.Win32;

using static application.Abstraction.EntityAbstraction;

namespace application.MVVM.ViewModel.Pages;

public partial class AddProductViewModel : ObservableObject
{
	private readonly IAddProductRepository _productRepository;

	public static event Action? CloseAddProduct;

	public List<string> SelectedFileNames { get; }
	private Guid _storageId;
	private Guid? _productId = null;

	[ObservableProperty]
	private string selectedFilePath = "Select File";

	[ObservableProperty]
	private string code;

	[ObservableProperty]
	private string title;

	[ObservableProperty]
	private string unit;

	[ObservableProperty]
	private double price;

	[ObservableProperty]
	private double availableForShipment;

	[ObservableProperty]
	private string party;

	[ObservableProperty]
	private DateTime implementationPeriod = DateTime.Now;

	[ObservableProperty]
	private DateTime expirationDate = DateTime.Now;

	[ObservableProperty]
	private byte[]? image;

	[ObservableProperty]
	private UserRole currentRole;

	public AddProductViewModel(IAddProductRepository productRepository)
	{
		_productRepository = productRepository;
		SelectedFileNames = [];
	}

	public void SetStorageId(Guid storageId)
	{
		_storageId = storageId;
		_productId = null;
	}

	public async void LoadProduct(Guid storageId, Guid productId)
	{
		_storageId = storageId;

		var product = await _productRepository.GetProduct(productId);
		_productId = product.Id;

		Code = product.Code;
		Title = product.Title;
		Unit = product.Unit;
		Price = product.Price;
		AvailableForShipment = product.Available_For_Shipment;
		Party = product.Party;
		ImplementationPeriod = product.Implementation_Period;
		ExpirationDate = product.Expiration_Date;
		Image = product.Image;
	}

	[RelayCommand]
	public void TriggerBackProduct()
	{
		CloseAddProduct?.Invoke();
	}
	[RelayCommand]
	private void SelectFile()
	{
		var openFileDialog = new OpenFileDialog
		{
			Filter = "Image Files (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg|All Files (*.*)|*.*",
			Multiselect = true
		};

		if (openFileDialog.ShowDialog() == true)
		{
			SelectedFileNames.Clear();
			SelectedFileNames.AddRange(openFileDialog.FileNames);

			if (SelectedFileNames.Any())
			{
				try
				{
					Image = ImageHelper.ConvertImageToByteArray(SelectedFileNames.First());
					SelectedFilePath = SelectedFileNames.First();
				}
				catch (Exception ex)
				{
					MessageBox.Show($"Error loading image: {ex.Message}");
					SelectedFilePath = "Error loading file";
				}
			}
		}
		else
		{
			SelectedFilePath = "Select File";
		}
	}
	//true if Add, false if Edit
	[RelayCommand]
	public async Task SaveProduct(string isAddOrEdit)
	{

		if (string.IsNullOrWhiteSpace(Code) ||
			string.IsNullOrWhiteSpace(Title) ||
			string.IsNullOrWhiteSpace(Unit) ||
			Price <= 0 ||
			AvailableForShipment <= 0 ||
			string.IsNullOrWhiteSpace(Party) ||
			ImplementationPeriod == default ||
			ExpirationDate == default)
		{
			MessageBox.Show("Please fill in all required fields.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
			return;
		}
		try
		{
			if (_storageId == Guid.Empty)
			{
				MessageBox.Show("Storage ID is not set. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			Guid id = _productId is null ? Guid.NewGuid() : (Guid)_productId;

			ProductModel product = new (
				  id,
				  EntityModel.OurUserModel.EntityId,
				  Code,
				  Title,
				  Unit,
				  Price,
				  Image,
				  _storageId,
				  AvailableForShipment,
				  Party,
				  ImplementationPeriod,
				  ExpirationDate
				 );

			if (Equals(isAddOrEdit, true.ToString()))
			{
				MessageBox.Show("isAdd");
				Guid productId = await _productRepository.InsertProduct(EntityModel.OurUserModel.EntityId, product);

				MessageBox.Show($"Product successfully added! ID: {productId}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

				ClearFields();
			}
			if (Equals(isAddOrEdit, false.ToString()))
			{
				MessageBox.Show("isEdit");
				await _productRepository.UpdateProduct(EntityModel.OurUserModel.EntityId, product);

				MessageBox.Show($"Product successfully updated!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

				TriggerBackProduct();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show($"Failed to add product: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
		}
	}

	private void ClearFields()
	{
		Code = string.Empty;
		Title = string.Empty;
		Unit = string.Empty;
		Price = 0;
		AvailableForShipment = 0;
		Party = string.Empty;
		ImplementationPeriod = DateTime.Now;
		ExpirationDate = DateTime.Now;
		Image = null;
		SelectedFilePath = "Select File";
	}
}
