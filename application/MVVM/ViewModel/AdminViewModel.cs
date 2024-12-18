using System.Diagnostics;
using System.Windows;
using System.Windows.Media.Animation;

using application.MVVM.View.AdminPages;
using application.MVVM.View.Pages;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Microsoft.Extensions.DependencyInjection;

namespace application.MVVM.ViewModel;

public partial class AdminViewModel : ObservableObject
{
	private readonly IServiceProvider _serviceProvider;

	[ObservableProperty]
	private object? currentView;

	public AdminViewModel(IServiceProvider serviceProvider)
	{
		_serviceProvider = serviceProvider;

		Account();
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
	private void Account() => CurrentView = _serviceProvider.GetRequiredService<AccountView>();
	[RelayCommand]
	private void Company() => CurrentView = _serviceProvider.GetRequiredService<CompanyView>();
	[RelayCommand]
	private void EntityManagers() => CurrentView = _serviceProvider.GetRequiredService<EntityManagersView>();
	[RelayCommand]
	private void EntityProductOrder() => CurrentView = _serviceProvider.GetRequiredService<EntityProductOrderView>();
	[RelayCommand]
	private void EntityStorage() => CurrentView = _serviceProvider.GetRequiredService<EntityStorageView>();
	[RelayCommand]
	private void Entity() => CurrentView = _serviceProvider.GetRequiredService<EntityView>();
	[RelayCommand]
	private void Order() => CurrentView = _serviceProvider.GetRequiredService<OrderView>();
	[RelayCommand]
	private void Product() => CurrentView = _serviceProvider.GetRequiredService<ProductView>();
	[RelayCommand]
	private void SupportTable() => CurrentView = _serviceProvider.GetRequiredService<View.AdminPages.SupportView>();
	[RelayCommand]
	private void User() => CurrentView = _serviceProvider.GetRequiredService<UserView>();

	[RelayCommand]
	private void Support() => CurrentView = _serviceProvider.GetRequiredService<SupportMainView>();
	[RelayCommand]
	private void Info() => CurrentView = _serviceProvider.GetRequiredService<InfoMainView>();
}
