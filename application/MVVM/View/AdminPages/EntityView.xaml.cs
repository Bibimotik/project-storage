using System.Windows.Controls;

using application.MVVM.ViewModel.AdminPages;

namespace application.MVVM.View.AdminPages;

/// <summary>
/// Логика взаимодействия для EntityView.xaml
/// </summary>
public partial class EntityView : UserControl
{
	public EntityView(EntityViewModel vm)
	{
		DataContext = vm;
		InitializeComponent();
	}
}
