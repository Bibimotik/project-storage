using System.Windows.Controls;

using application.MVVM.ViewModel.AdminPages;

namespace application.MVVM.View.AdminPages;

/// <summary>
/// Логика взаимодействия для SupportView.xaml
/// </summary>
public partial class SupportView : UserControl
{
	public SupportView(SupportViewModel vm)
	{
		DataContext = vm;
		InitializeComponent();
	}
}
