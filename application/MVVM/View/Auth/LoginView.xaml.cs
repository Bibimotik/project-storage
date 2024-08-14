using System.Text.RegularExpressions;
using System.Windows;
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
	
	private void Password_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
	{
		Regex inputRegex = new Regex(@"^[a-zA-Z0-9\s!.,/|<>{}`~;№%:?*()-=_+@#$^&]*$");
    
		Match match = inputRegex.Match(e.Text);
		if (!match.Success) 
		{
			e.Handled = true;
		}
	}
    
	private void Password_OnPreviewKeyDown(object sender, KeyEventArgs e)
	{
		if (e.Key == Key.Space)
		{
			e.Handled = true;
		}
	}
    
	private void CheckBox_Click1(object sender, RoutedEventArgs e)
	{
		var checkBox = sender as CheckBox;
		if (checkBox.IsChecked.Value)
		{
			passwordTextBox.Text = passwordPasswordBox.Password;
			passwordTextBox.Visibility = Visibility.Visible;
			passwordPasswordBox.Visibility = Visibility.Hidden;
		}
		else
		{
			passwordPasswordBox.Password = passwordTextBox.Text; 
			passwordTextBox.Visibility = Visibility.Hidden;
			passwordPasswordBox.Visibility = Visibility.Visible;
		}
	}
	// TODO - костыль ебнутый но пока будет так чтобы не тормозить
	private void PasswordPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
	{
		if (sender is PasswordBox passwordBox && passwordTextBox != null)
		{
			passwordTextBox.Text = passwordBox.Password;
		}
	}
}
