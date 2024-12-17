using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

using application.MVVM.Model;
using application.MVVM.ViewModel.Pages;

namespace application.MVVM.View.Pages;

public partial class StaffView : UserControl
{
	private readonly StaffViewModel _viewModel;
	private readonly ByteArrayToImageConverter _byteArrayToImageConverter = new ByteArrayToImageConverter();

	public StaffView(StaffViewModel viewModel)
	{
		_viewModel = viewModel;
		DataContext = viewModel;
		InitializeComponent();

		Loaded += StaffView_Loaded;
	}
	
	private async void StaffView_Loaded(object sender, RoutedEventArgs e)
	{
		await ReloadStaffData();
	}

	public async void LoadStaffData()
	{
		await _viewModel.LoadStaffAsync(EntityModel.OurUserModel.EntityId);
		StaffPanel.Children.Clear();

		foreach (var staffMember in _viewModel.StaffMembers)
		{
			var staffCard = CreateStaffCard(staffMember);
			StaffPanel.Children.Add(staffCard);
		}
	}

	private Border CreateStaffCard(StaffMember staffMember)
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
		
		image.Source = _byteArrayToImageConverter.Convert(staffMember.Image, typeof(BitmapImage), null, CultureInfo.InvariantCulture) as BitmapImage;
		
		Grid.SetColumn(image, 0);
		grid.Children.Add(image);

		var stackPanel = new StackPanel
		{
			Orientation = Orientation.Vertical
		};

		var fullName = $"{staffMember.FirstName} {staffMember.SecondName} {staffMember.ThirdName}";

		var idText = new TextBlock { Text = staffMember.Id.ToString(), Visibility = Visibility.Hidden };
		var nameText = new TextBlock { Text = $"Name: {fullName}", FontSize = 18 };
		var emailText = new TextBlock { Text = $"Email: {staffMember.Email}", FontSize = 16 };
		var accessText = new TextBlock { Text = $"Access: {staffMember.Access}", FontSize = 14 };

		stackPanel.Children.Add(nameText);
		stackPanel.Children.Add(emailText);
		stackPanel.Children.Add(accessText);

		Grid.SetColumn(stackPanel, 1);
		grid.Children.Add(stackPanel);

		border.Child = grid;

		return border;
	}
	
	private async Task ReloadStaffData()
	{
		await _viewModel.LoadStaffAsync(EntityModel.OurUserModel.EntityId);

		StaffPanel.Children.Clear();
		foreach (var storage in _viewModel.StaffMembers)
		{
			var storageCard = CreateStaffCard(storage);
			StaffPanel.Children.Add(storageCard);
		}
	}
}