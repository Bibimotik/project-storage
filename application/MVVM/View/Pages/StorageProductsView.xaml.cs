using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

using application.MVVM.Model;
using application.MVVM.ViewModel.Pages;

namespace application.MVVM.View.Pages
{
	public partial class StorageProductsView : UserControl
	{
		private readonly StorageProductsViewModel _viewModel;
		private DispatcherTimer _searchTimer;
		public Guid _storageId;

		public StorageProductsView(StorageProductsViewModel viewModel, Guid storageId)
		{
			_viewModel = viewModel;
			DataContext = viewModel;
			_storageId = storageId;
			InitializeComponent();

			LoadProductData(storageId);
			
			_searchTimer = new DispatcherTimer
			{
				Interval = TimeSpan.FromSeconds(0.5)
			};
			_searchTimer.Tick += OnSearchTimerTick;
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

		private Border CreateProductCard(ProductDataResult product)
		{
			var border = new Border
			{
				Style = (Style)FindResource("CardBorderStyle"),
				Margin = new Thickness(0, 10, 0, 10),
				Padding = new Thickness(10)
			};

			var productCard = new StackPanel
			{
				Orientation = Orientation.Vertical
			};

			var id = new TextBlock { Text = $"{product.Id}", Visibility = Visibility.Hidden };
			var titleText = new TextBlock { Text = $"Title: {product.Title}", FontSize = 20 };
			var priceText = new TextBlock { Text = $"Price: {product.Price:C}", FontSize = 16 };
			var codeText = new TextBlock { Text = $"Code: {product.Code}", FontSize = 14 };

			productCard.Children.Add(titleText);
			productCard.Children.Add(priceText);
			productCard.Children.Add(codeText);

			productCard.MouseLeftButtonUp += (sender, e) =>
			{
				//_viewModel.TriggerShowProduct(product.Code);
			};

			border.Child = productCard;

			return border;
		}
		
		private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			_searchTimer.Stop();
			_searchTimer.Start();
		}

		private async void OnSearchTimerTick(object sender, EventArgs e)
		{
			_searchTimer.Stop();

			var searchQuery = SearchTextBox.Text;

			await _viewModel.LoadProductsAsync(_storageId, searchQuery);

			StorageProductsPanel.Children.Clear();
			foreach (var storage in _viewModel.Products)
			{
				var storageCard = CreateProductCard(storage);
				StorageProductsPanel.Children.Add(storageCard);
			}
		}
	}
}
