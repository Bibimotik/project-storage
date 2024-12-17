using System.Windows;
using System.Windows.Controls;
using application.MVVM.Model;
using application.MVVM.ViewModel.Pages;

namespace application.MVVM.View.Pages;

public partial class StaffView : UserControl
{
	private readonly StaffViewModel _viewModel;

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

		var staffCard = new StackPanel
		{
			Orientation = Orientation.Vertical
		};

		var fullName = $"{staffMember.FirstName} {staffMember.SecondName} {staffMember.ThirdName}";

		var idText = new TextBlock { Text = staffMember.Id.ToString(), FontSize = 18 };
		var nameText = new TextBlock { Text = $"Name: {fullName}", FontSize = 18 };
		var emailText = new TextBlock { Text = $"Email: {staffMember.Email}", FontSize = 16 };
		var accessText = new TextBlock { Text = $"Access: {staffMember.Access}", FontSize = 14 };

		staffCard.Children.Add(nameText);
		staffCard.Children.Add(emailText);
		staffCard.Children.Add(accessText);
		
		staffCard.MouseLeftButtonUp += (sender, e) =>
		{
			//_viewModel.TriggerShowStorage(storage.Id);
		};

		border.Child = staffCard;

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