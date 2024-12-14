using System.Windows.Controls;

using application.MVVM.Model;
using application.MVVM.ViewModel.Pages;

namespace application.MVVM.View.Pages;

public partial class StorageProductsView : UserControl
{
	private readonly StorageProductsViewModel _viewModel;

	public StorageProductsView(StorageProductsViewModel viewModel, Guid storageId)
	{
		_viewModel = viewModel;
		DataContext = viewModel;
		InitializeComponent();

		LoadProductData(storageId);
	}

	private async void LoadProductData(Guid storageId)
	{
		await _viewModel.LoadProductsAsync(storageId);

		foreach (var product in _viewModel.Products)
		{
			var productCard = CreateProductCard(product);
			StorageProductsPanel.Children.Add(productCard);
		}
	}

	private StackPanel CreateProductCard(ProductDataResult product)
	{
		var productCard = new StackPanel
		{
			Orientation = Orientation.Vertical,
			Margin = new System.Windows.Thickness(0, 10, 0, 10),
			Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.LightBlue)
		};

		var titleText = new TextBlock { Text = $"Title: {product.Title}", FontSize = 20 };
		var priceText = new TextBlock { Text = $"Price: {product.Price:C}", FontSize = 16 };
		var codeText = new TextBlock { Text = $"Code: {product.Code}", FontSize = 14 };

		productCard.Children.Add(titleText);
		productCard.Children.Add(priceText);
		productCard.Children.Add(codeText);

		return productCard;
	}
}