using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using application.MVVM.Model;
using application.MVVM.ViewModel.Pages;

namespace application.MVVM.View.Pages
{
    public partial class RolesView : UserControl
    {
        private readonly RolesViewModel _viewModel;

        public RolesView(RolesViewModel viewModel)
        {
            _viewModel = viewModel;
            DataContext = viewModel;
            InitializeComponent();

            LoadRoles();
        }

        private async void LoadRoles()
        {
            string userId = "c60a90e1-da0a-4f02-bbf0-388052e0abdb";
            await _viewModel.LoadRolesAsync(Guid.Parse(userId));

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
                Margin = new System.Windows.Thickness(0, 10, 0, 10),
                Padding = new System.Windows.Thickness(10)
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
	            //_viewModel.TriggerShowRole(role.Id);
            };

            // Устанавливаем StackPanel как дочерний элемент Border
            border.Child = roleCard;

            return border;
        }
    }
}