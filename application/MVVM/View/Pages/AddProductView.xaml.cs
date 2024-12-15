using System.Windows;
using System.Windows.Controls;

using application.MVVM.ViewModel.Pages;

namespace application.MVVM.View.Pages;

public partial class AddProductView : UserControl
{
	private readonly AddProductViewModel _viewModel;
	public AddProductView(AddProductViewModel viewModel, Guid storageId)
	{
		_viewModel = viewModel;
		DataContext = viewModel;
		InitializeComponent();

		_viewModel.SetStorageId(storageId);
	}
}