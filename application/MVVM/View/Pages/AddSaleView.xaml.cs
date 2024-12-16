using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

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
	private void CheckBox_Checked(object sender, RoutedEventArgs e)
	{
		var checkBox = sender as CheckBox;
		if (checkBox != null && checkBox.Content is Guid productId)
		{
			Debug.WriteLine("qwe " + productId);
			_viewModel.ToggleSelection(productId);
		}
	}

	private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
	{
		var checkBox = sender as CheckBox;
		if (checkBox != null && checkBox.Content is Guid productId)
		{
			Debug.WriteLine("ewq " + productId);
			_viewModel.ToggleSelection(productId);
		}
	}
}