using System.Windows;

using application.MVVM.ViewModel;

namespace application.MVVM.View.Role;
/// <summary>
/// Логика взаимодействия для ManagerWindowView.xaml
/// </summary>
public partial class ManagerWindowView : Window
{
	public ManagerWindowView(ManagerWindowViewModel vm)
	{
		DataContext = vm;
		InitializeComponent();
	}

	private void RadioButton_Checked(object sender, RoutedEventArgs e)
	{
		//сюда что-нибудь с внешним видом можно дописать
	}
	private void Window_Loaded(object sender, RoutedEventArgs e)
	{
		// Перенос вызова команды после полной загрузки окна
		var viewModel = DataContext as ManagerWindowViewModel;
		viewModel?.OpenMenuCommand.Execute(null);
	}
}
