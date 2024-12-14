using System.Windows.Controls;

using application.MVVM.ViewModel.Pages;

namespace application.MVVM.View.Pages;

public partial class StaffView : UserControl
{
    public StaffView(StaffViewModel viewModel)
    {
	    DataContext = viewModel;
        InitializeComponent();
    }
}