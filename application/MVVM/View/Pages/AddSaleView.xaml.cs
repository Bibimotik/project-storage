using System.Windows.Controls;

using application.MVVM.ViewModel.Pages;

namespace application.MVVM.View.Pages;

public partial class AddSaleView : UserControl
{
	public AddSaleView(AddSaleViewModel viewModel)
	{
		DataContext = viewModel;
		InitializeComponent();
	}
}