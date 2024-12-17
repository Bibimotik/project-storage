using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
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
		private readonly ByteArrayToImageConverter _byteArrayToImageConverter = new ByteArrayToImageConverter();

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

		    var grid = new Grid();
		    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
		    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

		    var image = new Image
		    {
		        Width = 50,
		        Height = 50,
		        Margin = new Thickness(0, 0, 10, 0),
		        VerticalAlignment = VerticalAlignment.Center
		    };
		    
		    image.Source = _byteArrayToImageConverter.Convert(product.Image, typeof(BitmapImage), null, CultureInfo.InvariantCulture) as BitmapImage;

		    Grid.SetColumn(image, 0);
		    grid.Children.Add(image);

		    var stackPanel = new StackPanel
		    {
		        Orientation = Orientation.Vertical
		    };

		    var id = new TextBlock { Text = $"{product.Id}", Visibility = Visibility.Hidden };
		    var titleText = new TextBlock { Text = $"Title: {product.Title}", FontSize = 20 };
		    var priceText = new TextBlock { Text = $"Price: {product.Price:C} BYN", FontSize = 16 };
		    var codeText = new TextBlock { Text = $"Code: {product.Code}", FontSize = 14 };

		    stackPanel.Children.Add(titleText);
		    stackPanel.Children.Add(priceText);
		    stackPanel.Children.Add(codeText);

		    Grid.SetColumn(stackPanel, 1);
		    grid.Children.Add(stackPanel);

		    var deleteButton = new Button
		    {
		        Content = "Delete",
		        HorizontalAlignment = HorizontalAlignment.Right,
		        Margin = new Thickness(0, 10, 0, 0),
		        Width = 75,
		        Height = 30,
		        Style = (Style)FindResource("SendButtonRed")
		    };

		    deleteButton.Click += (sender, e) =>
		    {
		        MessageBox.Show($"Deleting product: {product.Id}");
		    };

		    Grid.SetColumn(deleteButton, 1);
		    grid.Children.Add(deleteButton);

		    border.Child = grid;

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
