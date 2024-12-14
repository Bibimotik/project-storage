using System.Windows.Controls;

using application.MVVM.ViewModel.Pages;

namespace application.MVVM.View.Pages;

public partial class StorageProductsView : UserControl
{
	public StorageProductsView(StorageProductsViewModel viewModel)
	{
		DataContext = viewModel;
		InitializeComponent();
	}
}