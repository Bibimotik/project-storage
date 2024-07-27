using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

using application.MVVM.View.Pages;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace application.MVVM.ViewModel;

public partial class MainViewModel : ObservableObject
{
	[ObservableProperty]
	private object? currentView;

	private bool isMenuExpanded = false;

	[RelayCommand]
	private void OpenMenu()
	{
		var window = Application.Current.MainWindow;
		var storyboard = (Storyboard)window.FindResource(isMenuExpanded ? "CollapseStoryboard" : "ExpandStoryboard");
		storyboard.Begin();
		isMenuExpanded = !isMenuExpanded;
	}

	//[RelayCommand]
	//private void Account()
	//{
	//	CurrentView = new Account();
	//}
	//[RelayCommand]
	//private void Statistics()
	//{
	//	CurrentView = new Statistics();
	//}
	//[RelayCommand]
	//private void Sales()
	//{
	//	CurrentView = new Sales();
	//}
	//[RelayCommand]
	//private void Storage()
	//{
	//	CurrentView = new Storage();
	//}
	//[RelayCommand]
	//private void Staff()
	//{
	//	CurrentView = new Staff();
	//}
	//[RelayCommand]
	//private void Support()
	//{
	//	CurrentView = new Support();
	//}
	//[RelayCommand]
	//private void Info()
	//{
	//	CurrentView = new Info();
	//}

	[RelayCommand]
	private void SwitchView(string viewName)
	{
		switch (viewName)
		{
			case "Account":
				CurrentView = new Account();
				break;
			case "Statistics":
				CurrentView = new Statistics();
				break;
			case "Sales":
				CurrentView = new Sales();
				break;
			case "Storage":
				CurrentView = new Storage();
				break;
			case "Staff":
				CurrentView = new Staff();
				break;
			case "Support":
				CurrentView = new Support();
				break;
			case "Info":
				CurrentView = new Info();
				break;
			default:
				throw new ArgumentException("Invalid view name", nameof(viewName));
		}
	}

	public MainViewModel()
	{
		//Account();
		CurrentView = new Account();
	}
}
