using System.Windows.Controls;

using application.MVVM.ViewModel.Pages;

namespace application.MVVM.View.Pages;
/// <summary>
/// Логика взаимодействия для AddStorageView.xaml
/// </summary>
public partial class AddStorageView : UserControl
{
	public AddStorageView(AddStorageViewModel viewModel)
	{
		DataContext = viewModel;
		InitializeComponent();
	}
}
