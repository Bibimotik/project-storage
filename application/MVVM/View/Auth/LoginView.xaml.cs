using System.Windows.Controls;
using System.Windows.Input;

namespace application.MVVM.View.Auth;
/// <summary>
/// Логика взаимодействия для LoginView.xaml
/// </summary>
public partial class LoginView : UserControl
{
	public LoginView()
	{
		InitializeComponent();
	}
	
	private void PasswordBox_OnPreviewKeyDown(object sender, KeyEventArgs e)
	{
		if (e.Key == Key.Space)
		{
			e.Handled = true;
		}
	}
}
