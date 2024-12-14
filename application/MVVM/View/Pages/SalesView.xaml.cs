using System.Windows.Controls;

using application.MVVM.ViewModel.Pages;

namespace application.MVVM.View.Pages;

public partial class SalesView : UserControl
{
    public SalesView(SalesViewModel viewModel)
    {
	    DataContext = viewModel;
        InitializeComponent();
    }
}