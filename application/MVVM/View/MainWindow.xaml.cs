using System.Windows;

using application.MVVM.ViewModel;

namespace application;

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
	private void Window_Loaded(object sender, RoutedEventArgs e)
	{
		// Перенос вызова команды после полной загрузки окна
		var viewModel = DataContext as MainViewModel;
		viewModel?.OpenMenuCommand.Execute(null);
	}
}