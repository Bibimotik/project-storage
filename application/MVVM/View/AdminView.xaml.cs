using System.Windows;

using application.MVVM.ViewModel;

namespace application.MVVM.View;

public partial class AdminView : Window
{
	public AdminView(AdminViewModel adminViewModel)
	{
		DataContext = adminViewModel;
		InitializeComponent();
	}

	private void RadioButton_Checked(object sender, RoutedEventArgs e)
	{
		//сюда что-нибудь с внешним видом можно дописать
	}
	private void Window_Loaded(object sender, RoutedEventArgs e)
	{
		// Перенос вызова команды после полной загрузки окна
		var viewModel = DataContext as AdminViewModel;
		viewModel?.OpenMenuCommand.Execute(null);
	}
}
