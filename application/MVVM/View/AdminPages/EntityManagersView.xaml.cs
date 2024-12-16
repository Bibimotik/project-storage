using System.Windows.Controls;

using application.MVVM.ViewModel.AdminPages;

namespace application.MVVM.View.AdminPages;

/// <summary>
/// Логика взаимодействия для EntityManagersView.xaml
/// </summary>
public partial class EntityManagersView : UserControl
{
	public EntityManagersView(EntityManagersViewModel vm)
	{
		DataContext = vm;
		InitializeComponent();
	}
}
