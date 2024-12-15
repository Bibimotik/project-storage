using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

using application.MVVM.Model;
using application.MVVM.ViewModel.Pages;

namespace application.MVVM.View.Pages
{
	public partial class StorageView : UserControl
	{
		private readonly StorageViewModel _viewModel;
		private DispatcherTimer _searchTimer;

		public StorageView(StorageViewModel viewModel)
		{
			_viewModel = viewModel;
			DataContext = viewModel;
			InitializeComponent();

			Loaded += StorageView_Loaded;

			_searchTimer = new DispatcherTimer
			{
				Interval = TimeSpan.FromSeconds(0.5)
			};
			_searchTimer.Tick += OnSearchTimerTick;
		}
		private async void StorageView_Loaded(object sender, RoutedEventArgs e)
		{
			await ReloadStorageData();
		}


		private async void LoadStorageData()
		{
			await _viewModel.LoadStorageAsync(EntityModel.OurUserModel.EntityId);

			foreach (var storage in _viewModel.Storage)
			{
				var storageCard = CreateStorageCard(storage);
				StoragePanel.Children.Add(storageCard);
			}
		}

		private Border CreateStorageCard(StorageDataResult storage)
		{
			var border = new Border
			{
				Style = (Style)FindResource("CardBorderStyle"),
				Margin = new Thickness(0, 10, 0, 10),
				Padding = new Thickness(10)
			};

			var storageCard = new StackPanel
			{
				Orientation = Orientation.Vertical
			};

			var id = new TextBlock { Text = $"{storage.Id}", Visibility = Visibility.Hidden };
			var pointText = new TextBlock { Text = $"Point: {storage.Point}", FontSize = 20 };
			var countryText = new TextBlock { Text = $"Country: {storage.Country}", FontSize = 16 };
			var cityText = new TextBlock { Text = $"City: {storage.City}", FontSize = 16 };
			var addressText = new TextBlock { Text = $"Address: {storage.Address}", FontSize = 14 };
			var indexText = new TextBlock { Text = $"Index: {storage.Index}", FontSize = 14 };

			storageCard.Children.Add(pointText);
			storageCard.Children.Add(countryText);
			storageCard.Children.Add(cityText);
			storageCard.Children.Add(addressText);
			storageCard.Children.Add(indexText);

			storageCard.MouseLeftButtonUp += (sender, e) =>
			{
				_viewModel.TriggerShowStorage(storage.Id);
			};

			border.Child = storageCard;

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

			await _viewModel.LoadStorageAsync(EntityModel.OurUserModel.EntityId, searchQuery);

			StoragePanel.Children.Clear();
			foreach (var storage in _viewModel.Storage)
			{
				var storageCard = CreateStorageCard(storage);
				StoragePanel.Children.Add(storageCard);
			}
		}

		private async void SortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			/*if (e.AddedItems.Count > 0)
	        {
		        var selectedOption = ((ComboBoxItem)e.AddedItems[0]).Content.ToString();
		        await _viewModel.OnSortChanged(selectedOption);
        
		        await ReloadStorageData();
	        }*/
		}

		private async Task ReloadStorageData()
		{
			await _viewModel.LoadStorageAsync(EntityModel.OurUserModel.EntityId);

			StoragePanel.Children.Clear();
			foreach (var storage in _viewModel.Storage)
			{
				var storageCard = CreateStorageCard(storage);
				StoragePanel.Children.Add(storageCard);
			}
		}
	}
}
