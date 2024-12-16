using System.Windows.Controls;

using application.MVVM.ViewModel.AdminPages;

namespace application.MVVM.View.AdminPages;

/// <summary>
/// Логика взаимодействия для EntityProductView.xaml
/// </summary>
public partial class ProductView : UserControl
{
	public ProductView(ProductViewModel vm)
	{
		DataContext = vm;
		InitializeComponent();
	}
}
