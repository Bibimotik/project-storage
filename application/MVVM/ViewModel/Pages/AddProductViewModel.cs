using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

using application.Abstraction.Interfaces;
using application.MVVM.Model;
using application.Utilities;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Microsoft.Win32;

namespace application.MVVM.ViewModel.Pages;

public partial class AddProductViewModel : ObservableObject
{
    private readonly IAddProductRepository _productRepository;
    public static event Action? OpenStorage;

    private Guid _storageId;

    public AddProductViewModel(IAddProductRepository productRepository)
    {
        _productRepository = productRepository;
        SelectedFileNames = new List<string>();
        SelectFileCommand = new RelayCommand(SelectFile);
    }

    public void SetStorageId(Guid storageId)
    {
        _storageId = storageId;
    }

    [RelayCommand]
    public void TriggerBackProduct()
    {
        OpenStorage?.Invoke();
    }

    public ICommand SelectFileCommand { get; }

    [ObservableProperty]
    private string selectedFilePath = "Select File";

    public List<string> SelectedFileNames { get; }
    
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
    
    [RelayCommand]
    public async Task AddProductAsync()
    {
	    MessageBox.Show(EntityModel.OurUserModel.EntityId.ToString());
	    try
	    {
		    if (_storageId == Guid.Empty)
		    {
			    MessageBox.Show("Storage ID is not set. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
			    return;
		    }

		    ProductModel product = new (
			    Guid.NewGuid(),
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

		    Guid productId = await _productRepository.InsertProduct(EntityModel.OurUserModel.EntityId, product);

		    MessageBox.Show($"Product successfully added! ID: {productId}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

		    ClearFields();
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
