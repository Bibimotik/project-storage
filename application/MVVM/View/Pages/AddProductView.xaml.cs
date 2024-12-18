using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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

		Label.Content = "Add product";
		EditButton.Visibility = Visibility.Collapsed;
	}

	public AddProductView(AddProductViewModel viewModel, Guid storageId, Guid productId)
	{
		_viewModel = viewModel;
		DataContext = viewModel;
		InitializeComponent();

		_viewModel.LoadProduct(storageId, productId);

		Label.Content = "Edit product";
		SaveButton.Visibility = Visibility.Collapsed;
	}
	private void Phone_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
	{
		Regex inputRegex = new Regex(@"^[0-9]+$");
		
		Match match = inputRegex.Match(e.Text);
		if (!match.Success) 
		{
			e.Handled = true;
		}
	}
        
	private void Space_OnPreviewKeyDown(object sender, KeyEventArgs e)
	{
		if (e.Key == Key.Space)
		{
			e.Handled = true;
		}
	}
}