using System.Collections.ObjectModel;
using System.Windows;

using application.Abstraction;
using application.Abstraction.Interfaces;
using application.MVVM.Model;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using CSharpFunctionalExtensions;

namespace application.MVVM.ViewModel.Pages;

public partial class AddSaleViewModel : ObservableObject
{
	private readonly IAddSaleRepository _saleRepository;
	private readonly IParserINNService _parserInnService;
	public static event Action? OpenSales;
	[ObservableProperty]
	private string inn;

	[ObservableProperty]
	private string kpp;

	[ObservableProperty]
	private string ogrn;

	[ObservableProperty]
	private string fullName;

	[ObservableProperty]
	private string address;

	[ObservableProperty]
	private string paymentAccount;

	[ObservableProperty]
	private string toBIK;

	[ObservableProperty]
	private string toBank;

	[ObservableProperty]
	private string toCorAccount;

	[ObservableProperty]
	private string fromCorAccount;

	[ObservableProperty]
	private string fromBIK;

	[ObservableProperty]
	private string fromBank;

	[ObservableProperty]
	private DateTime planDateShipment;

	[ObservableProperty]
	private string shippingAddress;

	[ObservableProperty]
	private DateTime applicationDate;
	
	[ObservableProperty]
	private Guid entityStorageId;

	[ObservableProperty]
	private string deliveryPoint;

	[ObservableProperty]
	private string deliveryAddress;

	[ObservableProperty]
	private DateTime planDateReceipt;

	[ObservableProperty]
	private string transporterFullName;

	[ObservableProperty]
	private string transporterShortName;

	[ObservableProperty]
	private string comment;

	[ObservableProperty]
	private double vat;

	public AddSaleViewModel(IAddSaleRepository addSaleRepository)
	{
		_saleRepository = addSaleRepository;
		PlanDateShipment = DateTime.Now;
		ApplicationDate = DateTime.Now;
		PlanDateReceipt = DateTime.Now;
	}
	
	[RelayCommand]
	public void TriggerSales() => OpenSales?.Invoke();
	
	[RelayCommand]
	public async Task GetParserDataINN(string inputINN)
	{
		var (parserData, error) = await _parserInnService.GetParserDataINN(Inn);

		if (!string.IsNullOrEmpty(error))
		{
			MessageBox.Show(error);
			return;
		}
		if (parserData != null)
		{
			Kpp = parserData.Kpp;
			FullName = parserData.FullName;
			Ogrn = parserData.Ogrn;
		}
	}
	
	[RelayCommand]
	public async Task GetProduct(string fullStorageName)
	{
		if (string.IsNullOrWhiteSpace(fullStorageName))
		{
			MessageBox.Show("Введите полное имя склада.");
			return;
		}

		try
		{
			var storageId = await _saleRepository.GetStorage(EntityModel.OurUserModel.EntityId, fullStorageName);

			EntityStorageId = storageId;
		}
		catch (InvalidOperationException ex)
		{
			MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
		}
		catch (Exception ex)
		{
			MessageBox.Show($"Произошла ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
		}
	}
	public ObservableCollection<ProductDataResult> Products { get; } = new();
	
	public async Task LoadProductsAsync(Guid storageId)
	{
		Products.Clear();

		var products = await _saleRepository.GetProductsDataAsync(storageId);

		foreach (var product in products)
		{
			Products.Add(product);
		}
	}
	
	[RelayCommand]
	private async Task LoadProducts()
	{
		if (EntityStorageId == Guid.Empty)
		{
			MessageBox.Show("Склад не найден. Проверьте введенные данные.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
			return;
		}

		try
		{
			await LoadProductsAsync(EntityStorageId);
		}
		catch (Exception ex)
		{
			MessageBox.Show($"Ошибка при загрузке продуктов: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
		}
	}
	
	partial void OnEntityStorageIdChanged(Guid value)
	{
		if (value != Guid.Empty)
		{
			LoadProductsCommand.Execute(entityStorageId);
		}
	}
}