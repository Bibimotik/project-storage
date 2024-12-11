using System.Windows;

using application.MVVM.ViewModel;

namespace application.MVVM.View;

public partial class AdminView : Window
{
	public AdminView(AdminViewModel adminViewModel)
	{
		DataContext = adminViewModel;
		InitializeComponent();
	}
}
