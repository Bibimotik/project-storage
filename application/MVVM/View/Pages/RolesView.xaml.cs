using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

using application.Abstraction;
using application.MVVM.Model;
using application.MVVM.ViewModel.Pages;
using application.Utilities.Converter;

using static application.Abstraction.EntityAbstraction;

namespace application.MVVM.View.Pages;

public partial class RolesView : UserControl
{
	private readonly RolesViewModel _viewModel;
	private readonly INavigationService _navigationService;
	private readonly ByteArrayToImageConverter _byteArrayToImageConverter = new();

	public RolesView(RolesViewModel viewModel, INavigationService navigationService)
	{
		_viewModel = viewModel;
		_navigationService = navigationService;
		DataContext = viewModel;
		InitializeComponent();

		Loaded += RolesView_Loaded;
	}

	private async void RolesView_Loaded(object sender, RoutedEventArgs e)
	{
		await ReloadRolesData();
	}

	private void Window_Loaded(object sender, RoutedEventArgs e)
	{
		var viewModel = DataContext as RolesViewModel;
		viewModel?.LoadRoleCommand.Execute(null);
	}

	private Border CreateRoleCard(RoleDataResult role)
	{
		var border = new Border
		{
			Style = (Style)FindResource("CardBorderStyle"),
			Margin = new Thickness(0, 10, 10, 10),
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

		image.Source = _byteArrayToImageConverter.Convert(role.Logo, typeof(BitmapImage), null, CultureInfo.InvariantCulture) as BitmapImage;

		Grid.SetColumn(image, 0);
		grid.Children.Add(image);

		var roleCard = new StackPanel
		{
			Orientation = Orientation.Vertical
		};

		var idText = new TextBlock { Text = role.Id.ToString(), FontSize = 1, Visibility = Visibility.Hidden};
		var firstText = new TextBlock { Text = role.First, FontSize = 20 };
		var secondText = new TextBlock { Text = role.Second, FontSize = 16 };
		var thirdText = new TextBlock { Text = role.Third, FontSize = 14 };
		var accessText = new TextBlock { Text = role.Access, FontSize = 14 };

		roleCard.Children.Add(idText);
		roleCard.Children.Add(firstText);
		roleCard.Children.Add(secondText);
		roleCard.Children.Add(thirdText);
		roleCard.Children.Add(accessText);

		roleCard.MouseLeftButtonUp += (sender, e) =>
		{
			if (role.Access == UserRole.Manager.GetDescription())
				_navigationService.ShowManagerRole();
			else if (role.Access == UserRole.Worker.GetDescription())
				_navigationService.ShowWorkerRole();
			else if (role.Access == UserRole.Analyst.GetDescription())
				_navigationService.ShowAnalystRole();
		};

		Grid.SetColumn(roleCard, 1);
		grid.Children.Add(roleCard);

		border.Child = grid;

		return border;
	}

	private async Task ReloadRolesData()
	{
		await _viewModel.LoadRolesAsync(EntityModel.OurUserModel.Id);

		RolesPanel.Children.Clear();
		foreach (var role in _viewModel.Roles)
		{
			var roleCard = CreateRoleCard(role);
			RolesPanel.Children.Add(roleCard);
		}
	}
}
