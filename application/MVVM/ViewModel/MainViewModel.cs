using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

using application.MVVM.Model;
using application.MVVM.View.Pages;
using application.MVVM.ViewModel.Pages;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Microsoft.Extensions.DependencyInjection;

using static application.Abstraction.EntityAbstraction;

namespace application.MVVM.ViewModel;

public partial class MainViewModel : ObservableObject
{
	private readonly IServiceProvider _serviceProvider;

	[ObservableProperty]
	private object? currentView;
	[ObservableProperty]
	private EntityType currentType;
	[ObservableProperty]
	private string shortName;
	[ObservableProperty]
	private string email;
	[ObservableProperty]
	private string menuIconPath = "../../Assets/Icons/Ava.png";
	[ObservableProperty]
	private object menuTag;

	public MainViewModel(IServiceProvider serviceProvider)
	{
		_serviceProvider = serviceProvider;

		Account();

		StorageViewModel.OpenAddStorage += OnOpenAddStorage;
		StorageViewModel.OpenStorageProducts += OnOpenStorageProducts;
		StorageViewModel.OpenEditStorage += OnOpenAddStorage;
		AddStorageViewModel.OpenStorage += OnOpenStorage;
		AddProductViewModel.CloseAddProduct += OnOpenStorage;
		StaffViewModel.OpenAddStaff += OnOpenAddStaff;
		AddStaffViewModel.OpenStaff += OnOpenStaff;
		StorageProductsViewModel.OpenAddProduct += OnOpenAddProduct;
		StorageProductsViewModel.CloseAddProduct += OnOpenStorage;
		StorageProductsViewModel.OpenEditProduct += OnOpenEditProduct;
		SalesViewModel.OpenAddSale += OnOpenAddSale;
		SalesViewModel.OpenEditSale += OnOpenAddSale;
		AddSaleViewModel.OpenSales += OnOpenSales;
		AddSaleViewModel.CloseAddSale += OnOpenSales;
		AccountViewModel.UpdateLogo += LoadUserData;
	}

	private bool isMenuExpanded = false;

	[RelayCommand]
	private void LoadUserData()
	{
		CurrentType = EntityModel.OurUserModel.EntityType;
		ShortName = EntityModel.OurUserModel.EntityType == EntityType.Company ?
			$"{EntityModel.OurUserModel.FullName} {EntityModel.OurUserModel.ShortName}" :
			$"{EntityModel.OurUserModel.FirstName} {EntityModel.OurUserModel.SecondName}";

		Email = EntityModel.OurUserModel.Email;

		UpdateMenuTag();
	}
	[RelayCommand]
	private void OpenMenu()
	{
		try
		{
			var window = Application.Current.MainWindow;
			var storyboard = window.TryFindResource(isMenuExpanded ? "CollapseStoryboard" : "ExpandStoryboard") as Storyboard;
			if (storyboard == null)
			{
				Debug.WriteLine("Storyboard not found");
				return;
			}
			storyboard.Begin();

			isMenuExpanded = !isMenuExpanded;

		}
		catch (Exception ex)
		{
			Console.WriteLine(ex);
			Debug.WriteLine(ex);
		}
	}
	[RelayCommand]
	private void Account() => CurrentView = _serviceProvider.GetRequiredService<AccountView>();
	[RelayCommand]
	private void Statistics() => CurrentView = _serviceProvider.GetRequiredService<StatisticsView>();
	[RelayCommand]
	private void Sales() => CurrentView = _serviceProvider.GetRequiredService<SalesView>();
	[RelayCommand]
	private void Storage() => CurrentView = _serviceProvider.GetRequiredService<StorageView>();
	[RelayCommand]
	private void Staff() => CurrentView = _serviceProvider.GetRequiredService<StaffView>();
	[RelayCommand]
	private void Roles() => CurrentView = _serviceProvider.GetRequiredService<RolesView>();
	[RelayCommand]
	private void Support() => CurrentView = _serviceProvider.GetRequiredService<SupportMainView>();
	[RelayCommand]
	private void Info() => CurrentView = _serviceProvider.GetRequiredService<InfoMainView>();

	private void OnOpenAddStorage() => CurrentView = _serviceProvider.GetRequiredService<AddStorageView>();
	private void OnOpenAddStorage(Guid storageId)
	{
		var viewModel = _serviceProvider.GetRequiredService<AddStorageViewModel>();
		CurrentView = new AddStorageView(viewModel, storageId);
	}

	private void OnOpenStorage() => CurrentView = _serviceProvider.GetRequiredService<StorageView>();
	private void OnOpenAddStaff() => CurrentView = _serviceProvider.GetRequiredService<AddStaffView>();
	private void OnOpenStaff() => CurrentView = _serviceProvider.GetRequiredService<StaffView>();
	private void OnOpenAddSale() => CurrentView = _serviceProvider.GetRequiredService<AddSaleView>();
	private void OnOpenAddSale(Guid saleId)
	{
		var viewModel = _serviceProvider.GetRequiredService<AddSaleViewModel>();
		CurrentView = new AddSaleView(viewModel, saleId);
	}

	private void OnOpenSales() => CurrentView = _serviceProvider.GetRequiredService<SalesView>();
	private void OnOpenStorageProducts(Guid storageId)
	{
		var viewModel = _serviceProvider.GetRequiredService<StorageProductsViewModel>();
		CurrentView = new StorageProductsView(viewModel, storageId);
	}
	private void OnOpenAddProduct(Guid storageId)
	{
		var viewModel = _serviceProvider.GetRequiredService<AddProductViewModel>();
		CurrentView = new AddProductView(viewModel, storageId);
	}
	private void OnOpenEditProduct(Guid storageId, Guid productId)
	{
		var viewModel = _serviceProvider.GetRequiredService<AddProductViewModel>();
		CurrentView = new AddProductView(viewModel, storageId, productId);
	}

	public bool IsNotCompanyVisible => CurrentType != EntityType.Company;

	partial void OnCurrentTypeChanged(EntityType value)
	{
		OnPropertyChanged(nameof(IsNotCompanyVisible));
	}

	private void UpdateMenuTag()
	{
		if (EntityModel.OurUserModel.Logo == null || EntityModel.OurUserModel.Logo.Length == 0)
			MenuTag = MenuIconPath; // Путь к стандартной иконке
		else
			MenuTag = ConvertLogoToImage(EntityModel.OurUserModel.Logo); // Преобразуем Logo в изображение
	}

	private ImageSource ConvertLogoToImage(byte[] logoBytes)
	{
		try
		{
			var image = new BitmapImage();
			using (var stream = new MemoryStream(logoBytes))
			{
				stream.Position = 0;
				image.BeginInit();
				image.CacheOption = BitmapCacheOption.OnLoad;
				image.StreamSource = stream;
				image.EndInit();
			}
			return image;
		}
		catch (Exception ex)
		{
			Debug.WriteLine($"Error converting logo to image: {ex.Message}");
			return null; // Возвращаем null, если произошла ошибка
		}
	}
}