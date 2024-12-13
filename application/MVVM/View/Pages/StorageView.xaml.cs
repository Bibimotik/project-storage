using System.Windows.Controls;

using application.MVVM.ViewModel.Pages;

namespace application.MVVM.View.Pages;

public partial class StorageView : UserControl
{
    public StorageView(StorageViewModel viewModel)
    {
	    DataContext = viewModel;
        InitializeComponent();
    }
}