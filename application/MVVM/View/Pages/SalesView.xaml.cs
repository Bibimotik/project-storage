using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

using application.MVVM.Model;
using application.MVVM.ViewModel.Pages;

using static application.Abstraction.EntityAbstraction;

namespace application.MVVM.View.Pages;

public partial class SalesView : UserControl
{
	private readonly SalesViewModel _viewModel;
	private DispatcherTimer _searchTimer;
	public SalesView(SalesViewModel viewModel)
	{
		_viewModel = viewModel;
		DataContext = _viewModel;
		InitializeComponent();

		Loaded += SalesView_Loaded;
		
		_searchTimer = new DispatcherTimer
		{
			Interval = TimeSpan.FromSeconds(0.5)
		};
		_searchTimer.Tick += OnSearchTimerTick;
	}

	private async void SalesView_Loaded(object sender, RoutedEventArgs e)
	{
		if (EntityModel.OurUserModel.Role is UserRole.Analyst)
			AddButton.IsEnabled = false;
		else
			AddButton.IsEnabled = true;

		await _viewModel.LoadOrdersAsync(EntityModel.OurUserModel.EntityId);
		PopulateSalesPanel();
	}

	private void PopulateSalesPanel()
	{
		SalePanel.Children.Clear();

		foreach (var order in _viewModel.Orders)
		{
			var card = CreateOrderCard(order);
			SalePanel.Children.Add(card);
		}
	}

	private Border CreateOrderCard(OrderModel order)
	{
		var border = new Border
		{
			Style = (Style)FindResource("CardBorderStyle"),
			Margin = new Thickness(0, 10, 0, 10),
			Padding = new Thickness(10)
		};
		
		var grid = new Grid();
		grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }); // Для текста
		grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) }); // Для кнопки
		
		var orderCard = new StackPanel
		{
			Orientation = Orientation.Vertical
		};
		
		var id = new TextBlock { Text = $"ID: {order.ID}", FontSize = 1, Visibility = Visibility.Hidden };
		var applicationDateText = new TextBlock { Text = $"Date: {order.Application_Date}" };
		var innText = new TextBlock { Text = $"INN: {order.INN}" };
		var kppText = new TextBlock { Text = $"KPP: {order.KPP}" };
		var fullNameText = new TextBlock { Text = $"Full name: {order.FullName}" };
		var addressText = new TextBlock { Text = $"Address: {order.Address}" };
		
		orderCard.Children.Add(id);
		orderCard.Children.Add(applicationDateText);
		orderCard.Children.Add(innText);
		orderCard.Children.Add(kppText);
		orderCard.Children.Add(fullNameText);
		orderCard.Children.Add(addressText);
		
		Grid.SetColumn(orderCard, 0);
		grid.Children.Add(orderCard);
		
		orderCard.MouseLeftButtonUp += (sender, e) =>
		{
			//_viewModel.TriggerShowProduct(product.Code);
		};
		
		var buttonPanel = new StackPanel
		{
			Orientation = Orientation.Vertical,
			VerticalAlignment = VerticalAlignment.Center,
			HorizontalAlignment = HorizontalAlignment.Right,
			Margin = new Thickness(10, 0, 0, 0)
		};
		
		var editButton = new Button
		{
			Content = "Edit",
			Width = 75,
			Height = 30,
			Style = (Style)FindResource("SendButton")
		};
		editButton.Click += (sender, e) =>
		{
			
		};
		
		var printButton = new Button
		{
			Content = "Print",
			HorizontalAlignment = HorizontalAlignment.Center,
			Width = 75,
			Height = 30,
			Style = (Style)FindResource("SendButton")
		};
		printButton.Click += async (sender, e) =>
		{
			await _viewModel.SelectAndProcessDocx(order.ID);
		};
		
		var deleteButton = new Button
		{
			Content = "Delete",
			HorizontalAlignment = HorizontalAlignment.Center,
			Width = 75,
			Height = 30,
			Style = (Style)FindResource("SendButtonRed")
		};
		deleteButton.Click += async (sender, e) =>
		{
			var result = MessageBox.Show($"Are you sure you want to delete this sale: {order.Application_Date}?", "Delete Sale", MessageBoxButton.YesNo);
			if (result == MessageBoxResult.Yes)
			{
				await _viewModel.DeleteStorage(order.ID);
				ReloadSalesData();
			}
		};

		if (EntityModel.OurUserModel.Role is UserRole.Analyst)
		{
			deleteButton.IsEnabled = false;
		}
		
		buttonPanel.Children.Add(editButton);
		buttonPanel.Children.Add(printButton);
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

		await _viewModel.LoadOrdersAsync(EntityModel.OurUserModel.EntityId, searchQuery);

		SalePanel.Children.Clear();
		foreach (var storage in _viewModel.Orders)
		{
			var storageCard = CreateOrderCard(storage);
			SalePanel.Children.Add(storageCard);
		}
	}
	
	private async void SortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
	{
		if (e.AddedItems.Count > 0)
		{
			var selectedItem = SortComboBox.SelectedItem as ComboBoxItem;

			string selectedOption = selectedItem?.Content?.ToString() ?? "Нет";

			await _viewModel.OnSortChanged(selectedOption);
	        
			await ReloadSalesData();
		}
	}
	
	private async Task ReloadSalesData()
	{
		await _viewModel.LoadOrdersAsync(EntityModel.OurUserModel.EntityId);

		SalePanel.Children.Clear();
		foreach (var storage in _viewModel.Orders)
		{
			var storageCard = CreateOrderCard(storage);
			SalePanel.Children.Add(storageCard);
		}
	}
}