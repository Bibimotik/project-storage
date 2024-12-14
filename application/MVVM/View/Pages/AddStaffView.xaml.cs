using System.Windows.Controls;

using application.MVVM.ViewModel.Pages;

namespace application.MVVM.View.Pages;

public partial class AddStaffView : UserControl
{
	public AddStaffView(AddStaffViewModel viewModel)
	{
		DataContext = viewModel;
		InitializeComponent();
	}
}