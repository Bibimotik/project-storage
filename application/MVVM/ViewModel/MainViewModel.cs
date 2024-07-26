using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace application.MVVM.ViewModel;

//[ObservableObject]
public partial class MainViewModel : ObservableObject
{
	[ObservableProperty]
	private string name;

	public MainViewModel()
	{
		Name = "bibimotik";
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
}
