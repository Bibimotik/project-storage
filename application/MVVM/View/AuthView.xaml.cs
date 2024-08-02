using System.Windows;

using application.MVVM.ViewModel;

namespace application.MVVM.View;
/// <summary>
/// Логика взаимодействия для AuthView.xaml
/// </summary>
public partial class AuthView : Window
{
	public AuthView(AuthViewModel authViewModel)
	{
		DataContext = authViewModel;
		InitializeComponent();
	}
}
