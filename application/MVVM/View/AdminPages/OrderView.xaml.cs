using System.Windows.Controls;

using application.MVVM.ViewModel.AdminPages;

namespace application.MVVM.View.AdminPages;

/// <summary>
/// Логика взаимодействия для OrderView.xaml
/// </summary>
public partial class OrderView : UserControl
{
	public OrderView(OrderViewModel vm)
	{
		DataContext = vm;
		InitializeComponent();
	}
}
