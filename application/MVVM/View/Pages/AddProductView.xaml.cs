using System.Windows.Controls;

using application.MVVM.ViewModel.Pages;

namespace application.MVVM.View.Pages;

public partial class AddProductView : UserControl
{
	public AddProductView(AddProductViewModel viewModel)
	{
		DataContext = viewModel;
		InitializeComponent();
	}
}