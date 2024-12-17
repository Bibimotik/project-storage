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

		Account();

		StorageViewModel.OpenAddStorage += OnOpenAddStorage;
		StorageViewModel.OpenStorageProducts += OnOpenStorageProducts;
		AddStorageViewModel.OpenStorage += OnOpenStorage;
		AddProductViewModel.OpenStorage += OnOpenStorage;
		StaffViewModel.OpenAddStaff += OnOpenAddStaff;
		AddStaffViewModel.OpenStaff += OnOpenStaff;
		StorageProductsViewModel.OpenAddProduct += OnOpenAddProduct;
		StorageProductsViewModel.CloseAddProduct += OnOpenStorage;
		SalesViewModel.OpenAddSale += OnOpenAddSale;
		AddSaleViewModel.OpenSales += OnOpenSales;
	}

	private bool isMenuExpanded = false;

	[RelayCommand]
	private void OpenMenu()
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
	[RelayCommand]
	private void Account() => CurrentView = _serviceProvider.GetRequiredService<AccountView>();
	[RelayCommand]
	private void Sales() => CurrentView = _serviceProvider.GetRequiredService<SalesView>();
	[RelayCommand]
	private void Storage() => CurrentView = _serviceProvider.GetRequiredService<StorageView>();
	[RelayCommand]
	private void Roles() => CurrentView = _serviceProvider.GetRequiredService<RolesView>();
	[RelayCommand]
	private void Support() => CurrentView = _serviceProvider.GetRequiredService<SupportMainView>();
	[RelayCommand]
	private void Info() => CurrentView = _serviceProvider.GetRequiredService<InfoMainView>();

	private void OnOpenAddStorage() => CurrentView = _serviceProvider.GetRequiredService<AddStorageView>();
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
}