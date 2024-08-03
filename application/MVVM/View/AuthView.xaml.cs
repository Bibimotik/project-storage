using System.Windows;

using application.MVVM.ViewModel;

namespace application.MVVM.View;

public partial class AuthView : Window
{
	public AuthView(AuthViewModel authViewModel)
	{
		DataContext = authViewModel;
		InitializeComponent();
	}
}
