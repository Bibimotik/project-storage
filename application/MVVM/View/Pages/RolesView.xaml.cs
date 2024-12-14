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

            // После получения данных обновляем UI
            foreach (var role in _viewModel.Roles)
            {
                var roleCard = CreateRoleCard(role);
                RolesPanel.Children.Add(roleCard);
            }
        }

        // Метод для создания карточки роли
        private StackPanel CreateRoleCard(RoleDataResult role)
        {
            var roleCard = new StackPanel
            {
                Orientation = Orientation.Vertical,
                Margin = new System.Windows.Thickness(0, 10, 0, 10),
                Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.LightGray)
            };

            var id = new TextBlock { Text = role.Id.ToString(), FontSize = 20, Visibility = Visibility.Hidden};
            var firstText = new TextBlock { Text = role.First, FontSize = 20 };
            var secondText = new TextBlock { Text = role.Second, FontSize = 16 };
            var thirdText = new TextBlock { Text = role.Third, FontSize = 14 };
            var accessText = new TextBlock { Text = role.Access, FontSize = 14 };

            roleCard.Children.Add(firstText);
            roleCard.Children.Add(secondText);
            roleCard.Children.Add(thirdText);
            roleCard.Children.Add(accessText);

            return roleCard;
        }
    }
}
