using System.Diagnostics;
using System.Windows;
using System.Windows.Media.Animation;

using application.MVVM.View.Pages;
using application.MVVM.ViewModel.Pages;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Microsoft.Extensions.DependencyInjection;

namespace application.MVVM.ViewModel.Roles;

public partial class WorkerWindowViewModel : ObservableObject
{
	private readonly IServiceProvider _serviceProvider;

	[ObservableProperty]
	private object? currentView;

	public WorkerWindowViewModel(IServiceProvider serviceProvider)
	{
		_serviceProvider = serviceProvider;

		Storage();

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
		AddSaleViewModel.OpenSales += OnOpenSales;
	}

	private bool isMenuExpanded = false;

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
	private void Storage() => CurrentView = _serviceProvider.GetRequiredService<StorageView>();
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
}