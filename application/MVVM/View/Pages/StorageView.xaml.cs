using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

using application.MVVM.Model;
using application.MVVM.ViewModel.Pages;

using static application.Abstraction.EntityAbstraction;

namespace application.MVVM.View.Pages;

public partial class StorageView : UserControl
{
	private readonly StorageViewModel _viewModel;
	private readonly DispatcherTimer _searchTimer;

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
		if (EntityModel.OurUserModel.Role is UserRole.Worker or 
			UserRole.Analyst or
			UserRole.Manager)
			AddButton.IsEnabled = false;
		else
			AddButton.IsEnabled = true;

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
			Margin = new Thickness(0, 10, 10, 10),
			Padding = new Thickness(10)
		};

		var grid = new Grid();
		grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }); // Для текста
		grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) }); // Для кнопки

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

		Grid.SetColumn(storageCard, 0);
		grid.Children.Add(storageCard);

		storageCard.MouseLeftButtonUp += (sender, e) =>
		{
			_viewModel.TriggerShowStorage(storage.Id);
		};

		var buttonPanel = new StackPanel
		{
			Orientation = Orientation.Vertical,
			VerticalAlignment = VerticalAlignment.Center,
			HorizontalAlignment = HorizontalAlignment.Right,
			Margin = new Thickness(10, 0, 0, 0)
		};

		var deleteButton = new Button
		{
			Content = "Delete",
			HorizontalAlignment = HorizontalAlignment.Center,
			Margin = new Thickness(0, 0, 0, 0),
			Width = 75,
			Height = 30,
			Style = (Style)FindResource("SendButtonRed")
		};
		deleteButton.Click += async (sender, e) =>
		{
			var result = MessageBox.Show($"Are you sure you want to delete this storage: {storage.Point}?", "Delete Storage", MessageBoxButton.YesNo);
			if (result == MessageBoxResult.Yes)
			{
				await _viewModel.DeleteStorage(storage.Id);
				ReloadStorageData();
			}
		};

		var editButton = new Button
		{
			Content = "Edit",
			Margin = new Thickness(0, -36, 0, 10),
			Width = 75,
			Height = 30,
			Style = (Style)FindResource("SendButton")
		};
		editButton.Click += (sender, e) =>
		{
			_viewModel.EditStorage(storage.Id);
		};

		if (EntityModel.OurUserModel.Role is UserRole.Worker or
			UserRole.Analyst or
			UserRole.Manager)
		{
			deleteButton.IsEnabled = false;
			editButton.IsEnabled = false;
		}

		buttonPanel.Children.Add(editButton);
		buttonPanel.Children.Add(deleteButton);

		Grid.SetColumn(buttonPanel, 1);
		grid.Children.Add(buttonPanel);

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
		if (e.AddedItems.Count > 0)
        {
	        var selectedItem = SortComboBox.SelectedItem as ComboBoxItem;

	        string selectedOption = selectedItem?.Content?.ToString() ?? "Нет";

	        await _viewModel.OnSortChanged(selectedOption);
	        
	        await ReloadStorageData();
        }
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
