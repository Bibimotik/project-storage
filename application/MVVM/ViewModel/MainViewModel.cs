using System.Windows;
using System.Windows.Media.Animation;

using application.MVVM.View.Pages;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Microsoft.Extensions.DependencyInjection;

namespace application.MVVM.ViewModel;

public partial class MainViewModel : ObservableObject
{
	private readonly IServiceProvider _serviceProvider;

	[ObservableProperty]
	private object? currentView;

	public MainViewModel(IServiceProvider serviceProvider)
	{
		_serviceProvider = serviceProvider;
		Account();
	}

	private bool isMenuExpanded = false;

	[RelayCommand]
	private void OpenMenu()
	{
		var window = Application.Current.MainWindow;
		var storyboard = (Storyboard)window.FindResource(isMenuExpanded ? "CollapseStoryboard" : "ExpandStoryboard");
		storyboard.Begin();
		isMenuExpanded = !isMenuExpanded;
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
	private void Support() => CurrentView = _serviceProvider.GetRequiredService<SupportView>();
	[RelayCommand]
	private void Info() => CurrentView = _serviceProvider.GetRequiredService<InfoView>();
}
