using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Windows;

using application.Abstraction;
using application.Abstraction.Interfaces;
using application.MVVM.Model;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Microsoft.Win32;

using static application.Abstraction.EntityAbstraction;

namespace application.MVVM.ViewModel.Pages;

public partial class AddSaleViewModel : ObservableObject
{
	private readonly IAddSaleRepository _addSaleRepository;
	private readonly ISaleRepository _saleRepository;
	private readonly IParserINNService _parserInnService;

	public static event Action? OpenSales;
	public static event Action? CloseAddSale;

	private Guid? _saleId = null;

	public ObservableCollection<ProductDataResult> Products { get; } = [];
	private readonly ObservableCollection<Guid> _products = [];

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
	private int vat;
	[ObservableProperty]
	private string fromInn;
	[ObservableProperty]
	private string fromKpp;
	[ObservableProperty]
	private string fromOgrn;
	[ObservableProperty]
	private string fromName;
	[ObservableProperty]
	private string genDir;

	public AddSaleViewModel(IAddSaleRepository addSaleRepository, ISaleRepository saleRepository, IParserINNService parserInnService)
	{
		_addSaleRepository = addSaleRepository;
		_saleRepository = saleRepository;
		_parserInnService = parserInnService;
		PlanDateShipment = DateTime.Now;
		ApplicationDate = DateTime.Now;
		PlanDateReceipt = DateTime.Now;
	}

	public void LoadSale()
	{
		_saleId = null;
	}

	public async void LoadSale(Guid saleId)
	{
		var sale = await _saleRepository.GetOrderByIdAsync(saleId);
		_saleId = saleId;

		Inn = sale.INN;
		Kpp = sale.KPP;
		Ogrn = sale.OGRN;
		FullName = sale.FullName;
		Address = sale.Address;
		PaymentAccount = sale.Payment_Account;
		ToBIK = sale.ToBIK;
		ToBank = sale.ToBank;
		ToCorAccount = sale.ToCor_Account;
		FromCorAccount = sale.FromCor_Account;
		FromBIK = sale.FromBIK;
		FromBank = sale.FromBank;
		PlanDateShipment = sale.Plan_Date_Shipment;
		ShippingAddress = sale.Shipping_Address;
		ApplicationDate = sale.Application_Date;
		EntityStorageId = sale.Entity_Storage_ID;
		DeliveryPoint = sale.Delivery_Point;
		DeliveryAddress = sale.Delivery_Address;
		PlanDateReceipt = sale.Plan_Date_Receipt;
		TransporterFullName = sale.TransporterFullName;
		TransporterShortName = sale.TransporterShortName;
		Comment = sale.Comment;
		Vat = (int)sale.VAT;
		//FromInn = sale.frominn;
		//FromKpp = sale.fromkpp;
		//FromOgrn = sale.fromo;
		//FromName = sale.Fromna;
		//GenDir = sale.gend;
	}

	[RelayCommand]
	private void Back()
	{
		CloseAddSale?.Invoke();
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
			var storageId = await _addSaleRepository.GetStorage(EntityModel.OurUserModel.EntityId, fullStorageName);

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

	[RelayCommand]
	private void ViewData()
	{
		StringBuilder str = new();
		foreach (Guid id in _products)
		{
			Debug.WriteLine(id.ToString());
			str.AppendFormat(id.ToString());
		}
		MessageBox.Show(str.ToString());
	}

	public async Task LoadProductsAsync(Guid storageId)
	{
		Products.Clear();

		var products = await _addSaleRepository.GetProductsDataAsync(storageId);

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

	public void ToggleSelection(Guid productId)
	{
		Debug.WriteLine("select " + productId.ToString());

		if (_products.Contains(productId))
			_products.Remove(productId);
		else
			_products.Add(productId);
	}

	[RelayCommand]
	public async Task SaveSale()
	{
		try
		{
			if (string.IsNullOrWhiteSpace(Inn) ||
					string.IsNullOrWhiteSpace(Kpp) ||
					string.IsNullOrWhiteSpace(Ogrn) ||
					string.IsNullOrWhiteSpace(FullName) ||
					string.IsNullOrWhiteSpace(Address) ||
					string.IsNullOrWhiteSpace(PaymentAccount) ||
					string.IsNullOrWhiteSpace(ToCorAccount) ||
					string.IsNullOrWhiteSpace(ToBIK) ||
					string.IsNullOrWhiteSpace(ToBank) ||
					string.IsNullOrWhiteSpace(FromCorAccount) ||
					string.IsNullOrWhiteSpace(FromBIK) ||
					string.IsNullOrWhiteSpace(FromBank) ||
					PlanDateShipment == null ||
					string.IsNullOrWhiteSpace(ShippingAddress) ||
					ApplicationDate == null ||
					string.IsNullOrWhiteSpace(DeliveryPoint) ||
					string.IsNullOrWhiteSpace(DeliveryAddress) ||
					PlanDateReceipt == null ||
					string.IsNullOrWhiteSpace(Comment) ||
					Vat == null)
			{
				MessageBox.Show("Заполните все поля");
				return;
			}

			Guid id = _saleId is null ? Guid.NewGuid() : (Guid)_saleId;

			Guid entityManagerId = EntityModel.OurUserModel.Role is UserRole.NoRole ?
				Guid.Empty :
				EntityModel.OurUserModel.EntityId;

			var orderModel = new OrderModel
			{
				ID = id,
				INN = Inn,
				KPP = Kpp,
				OGRN = Ogrn,
				FullName = FullName,
				Address = Address,
				Payment_Account = PaymentAccount,
				ToCor_Account = ToCorAccount,
				ToBIK = ToBIK,
				ToBank = ToBank,
				FromCor_Account = FromCorAccount,
				FromBIK = FromBIK,
				FromBank = FromBank,
				Plan_Date_Shipment = PlanDateShipment,
				Shipping_Address = ShippingAddress,
				Application_Date = ApplicationDate,
				Delivery_Point = DeliveryPoint,
				Delivery_Address = DeliveryAddress,
				Plan_Date_Receipt = PlanDateReceipt,
				TransporterFullName = TransporterFullName,
				TransporterShortName = TransporterShortName,
				Comment = Comment,
				VAT = Vat
			};

			var upd = await _addSaleRepository.UpdateOrder(orderModel);

			if (upd)
				MessageBox.Show("Заказ успешно обновлен!");

		}
		catch (Exception ex)
		{
			MessageBox.Show($"Произошла ошибка при добавлении заказа: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
		}
	}
	[RelayCommand]
	public async Task InsertOrder()
	{
		if (_products.Count == 0)
		{
			MessageBox.Show("Выберите хотя бы один продукт.");
			return;
		}

		try
		{
			if (string.IsNullOrWhiteSpace(Inn) ||
				string.IsNullOrWhiteSpace(Kpp) ||
				string.IsNullOrWhiteSpace(Ogrn) ||
				string.IsNullOrWhiteSpace(FullName) ||
				string.IsNullOrWhiteSpace(Address) ||
				string.IsNullOrWhiteSpace(PaymentAccount) ||
				string.IsNullOrWhiteSpace(ToCorAccount) ||
				string.IsNullOrWhiteSpace(ToBIK) ||
				string.IsNullOrWhiteSpace(ToBank) ||
				string.IsNullOrWhiteSpace(FromCorAccount) ||
				string.IsNullOrWhiteSpace(FromBIK) ||
				string.IsNullOrWhiteSpace(FromBank) ||
				PlanDateShipment == null ||
				string.IsNullOrWhiteSpace(ShippingAddress) ||
				ApplicationDate == null ||
				string.IsNullOrWhiteSpace(DeliveryPoint) ||
				string.IsNullOrWhiteSpace(DeliveryAddress) ||
				PlanDateReceipt == null ||
				string.IsNullOrWhiteSpace(Comment) ||
				Vat == null)
			{
				MessageBox.Show("Заполните все поля");
				return;
			}

			Guid entityManagerId = EntityModel.OurUserModel.Role is UserRole.NoRole ?
				Guid.Empty :
				EntityModel.OurUserModel.EntityId;

			var orderModel = new OrderModel
			{
				ID = Guid.NewGuid(),
				Entity_ID = EntityModel.OurUserModel.EntityId,
				Entity_Managers_ID = entityManagerId,
				INN = Inn,
				KPP = Kpp,
				OGRN = Ogrn,
				FullName = FullName,
				Address = Address,
				Payment_Account = PaymentAccount,
				ToCor_Account = ToCorAccount,
				ToBIK = ToBIK,
				ToBank = ToBank,
				FromCor_Account = FromCorAccount,
				FromBIK = FromBIK,
				FromBank = FromBank,
				Plan_Date_Shipment = PlanDateShipment,
				Shipping_Address = ShippingAddress,
				Application_Date = ApplicationDate,
				Delivery_Point = DeliveryPoint,
				Delivery_Address = DeliveryAddress,
				Plan_Date_Receipt = PlanDateReceipt,
				TransporterFullName = TransporterFullName,
				TransporterShortName = TransporterShortName,
				Comment = Comment,
				VAT = Vat
			};

			var orderId = await _addSaleRepository.InsertOrder(orderModel);

			foreach (var productId in _products)
			{
				var productOrderModel = new ProductOrderModel
				{
					ID = Guid.NewGuid(),
					Product_ID = productId,
					Order_ID = orderId,
					Count = Vat
				};

				await _addSaleRepository.InsertProductOrder(productOrderModel);
			}

			MessageBox.Show("Заказ успешно добавлен!");

			var result = MessageBox.Show("Печатать отчет?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

			if (result == MessageBoxResult.Yes)
			{
				var openFileDialog = new OpenFileDialog
				{
					Title = "Выберите шаблон документа",
					Filter = "Документы Word (*.docx)|*.docx|Все файлы (*.*)|*.*",
					CheckFileExists = true,
					CheckPathExists = true
				};

				if (openFileDialog.ShowDialog() == true)
				{
					try
					{
						var companyData = await _addSaleRepository.GetCompanyDataByEntityId(EntityModel.OurUserModel.EntityId);
						FromInn = companyData.INN;
						FromKpp = companyData.KPP;
						FromOgrn = companyData.OGRN;
						FromName = companyData.FullName;
						GenDir = companyData.Director;
					}
					catch (Exception ex)
					{
						MessageBox.Show($"Ошибка при получении данных компании: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
						return;
					}
					string selectedFilePath = openFileDialog.FileName;
					await RunPythonScript(selectedFilePath);
				}
			}
			else
			{
				Debug.WriteLine("Отчет не выбран для печати.");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show($"Произошла ошибка при добавлении заказа: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
		}
	}

	private async Task RunPythonScript(string templateDocxPath)
	{
		try
		{
			string pythonExecutable = "python";
			string pythonScriptPath = @"..\..\..\..\parser\docxFile\main.py";

			string arguments = string.Join(" ", new[]
			{
				$"\"{templateDocxPath}\"",
				$"\"{PlanDateShipment.ToString("dd MMMM yyyy")}\"",
				$"\"{FromName}\"",
				$"\"{GenDir}\"",
				$"\"{FullName}\"",
				$"\"{ShippingAddress}\"",
				$"\"{FromInn}\"",
				$"\"{FromOgrn}\"",
				$"\"{FromCorAccount}\"",
				$"\"{FromCorAccount}\"",
				$"\"{FromBIK}\"",
				$"\"{FromBank}\"",
				$"\"{DeliveryPoint}\"",
				$"\"{Inn}\"",
				$"\"{Ogrn}\"",
				$"\"{ToCorAccount}\"",
				$"\"{ToCorAccount}\"",
				$"\"{ToBIK}\"",
				$"\"{ToBank}\"",
				$"\"{FromKpp}\"",
				$"\"{Kpp}\"",
				$"\"{DeliveryAddress}\""
			});

			var processStartInfo = new ProcessStartInfo
			{
				FileName = pythonExecutable,
				Arguments = $"\"{pythonScriptPath}\" {arguments}",
				RedirectStandardOutput = true,
				RedirectStandardError = true,
				UseShellExecute = false,
				CreateNoWindow = true
			};

			using var process = Process.Start(processStartInfo);
			if (process == null)
			{
				throw new InvalidOperationException("Не удалось запустить Python-скрипт.");
			}

			string output = await process.StandardOutput.ReadToEndAsync();
			string errors = await process.StandardError.ReadToEndAsync();

			process.WaitForExit();

			if (!string.IsNullOrEmpty(errors))
			{
				MessageBox.Show($"Ошибка при выполнении скрипта:\n{errors}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
			}
			else
			{
				MessageBox.Show("Документ успешно сгенерирован и сохранён в папке Загрузки.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show($"Ошибка при запуске скрипта: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
		}
	}
}
