using System.Windows;

using application.MVVM.ViewModel;

namespace application;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
	public MainWindow(MainViewModel mainViewModel)
	{
		DataContext = mainViewModel;
		InitializeComponent();
	}
	
	private void RadioButton_Checked(object sender, RoutedEventArgs e)
	{
		//сюда что-нибудь с внешним видом можно дописать
	}
}