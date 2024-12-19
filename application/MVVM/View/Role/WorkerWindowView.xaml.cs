using System.Windows;

using application.MVVM.ViewModel.Roles;

namespace application.MVVM.View.Role;
/// <summary>
/// Логика взаимодействия для WorkerWindowView.xaml
/// </summary>
public partial class WorkerWindowView : Window
{
	public WorkerWindowView(WorkerWindowViewModel vm)
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
		var viewModel = DataContext as WorkerWindowViewModel;
		viewModel?.OpenMenuCommand.Execute(null);
	}
}
