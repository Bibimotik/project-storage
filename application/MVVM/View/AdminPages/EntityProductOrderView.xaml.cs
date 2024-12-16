using System.Windows.Controls;

using application.MVVM.ViewModel.AdminPages;

namespace application.MVVM.View.AdminPages;

/// <summary>
/// Логика взаимодействия для ProductOrderView.xaml
/// </summary>
public partial class EntityProductOrderView : UserControl
{
	public EntityProductOrderView(EntityProductOrderViewModel vm)
	{
		DataContext = vm;
		InitializeComponent();
	}
}
