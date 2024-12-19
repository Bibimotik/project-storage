using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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

		viewModel.LoadSale();
		Label.Content = "Add sale";
		EditButton.Visibility = Visibility.Collapsed;
	}

	public AddSaleView(AddSaleViewModel viewModel, Guid saleId)
	{
		_viewModel = viewModel;
		DataContext = viewModel;
		InitializeComponent();

		viewModel.LoadSale(saleId);
		Label.Content = "Edit sale";
		SaveButton.Visibility = Visibility.Collapsed;
		//ScrollViewer.Visibility = Visibility.Collapsed;
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