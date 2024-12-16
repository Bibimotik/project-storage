using System.Windows;
using System.Windows.Controls;

using application.MVVM.Model;
using application.MVVM.ViewModel.Pages;

namespace application.MVVM.View.Pages;

public partial class AddSaleView : UserControl
{
	private readonly AddSaleViewModel _viewModel;
	public AddSaleView(AddSaleViewModel viewModel)
	{
		_viewModel = viewModel;
		DataContext = viewModel;
		InitializeComponent();
	}
}