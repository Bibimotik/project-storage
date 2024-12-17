using System.Windows;
using System.Windows.Controls;

using application.Abstraction;
using application.MVVM.Model;
using application.MVVM.ViewModel;
using application.MVVM.ViewModel.Pages;

using static application.Abstraction.EntityAbstraction;

namespace application.MVVM.View.Pages;

public partial class RolesView : UserControl
{
	private readonly RolesViewModel _viewModel;
	private readonly INavigationService _navigationService;

	public RolesView(RolesViewModel viewModel, INavigationService navigationService)
	{
		_viewModel = viewModel;
		_navigationService = navigationService;
		DataContext = viewModel;
		InitializeComponent();

		LoadRoles();
	}

	private void Window_Loaded(object sender, RoutedEventArgs e)
	{
		// Перенос вызова команды после полной загрузки окна
		var viewModel = DataContext as RolesViewModel;
		viewModel?.LoadRoleCommand.Execute(null);
	}

	private async void LoadRoles()
	{
		await _viewModel.LoadRolesAsync(EntityModel.OurUserModel.Id);

		foreach (var role in _viewModel.Roles)
		{
			var roleCard = CreateRoleCard(role);
			RolesPanel.Children.Add(roleCard);
		}
	}

	private Border CreateRoleCard(RoleDataResult role)
	{
		var border = new Border
		{
			Style = (Style)FindResource("CardBorderStyle"),
			Margin = new Thickness(0, 10, 0, 10),
			Padding = new Thickness(10)
		};

		var roleCard = new StackPanel
		{
			Orientation = Orientation.Vertical
		};

		var firstText = new TextBlock { Text = role.First, FontSize = 20 };
		var secondText = new TextBlock { Text = role.Second, FontSize = 16 };
		var thirdText = new TextBlock { Text = role.Third, FontSize = 14 };
		var accessText = new TextBlock { Text = role.Access, FontSize = 14 };

		roleCard.Children.Add(firstText);
		roleCard.Children.Add(secondText);
		roleCard.Children.Add(thirdText);
		roleCard.Children.Add(accessText);
		
		roleCard.MouseLeftButtonUp += (sender, e) =>
		{
			if(role.Access == UserRole.Manager.GetDescription())
				_navigationService.ShowManagerRole();
			else if(role.Access == UserRole.Worker.GetDescription())
				_navigationService.ShowWorkerRole();
			else if(role.Access == UserRole.Analyst.GetDescription())
				_navigationService.ShowAnalystRole();
			//_viewModel.TriggerShowRole(role.Id);
		};

		// Устанавливаем StackPanel как дочерний элемент Border
		border.Child = roleCard;

		return border;
	}
}