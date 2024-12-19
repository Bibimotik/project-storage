using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using application.MVVM.ViewModel.Pages;

namespace application.MVVM.View.Pages;

public partial class AccountView : UserControl
{
	public AccountView(AccountViewModel accountViewModel)
	{
		DataContext = accountViewModel;
		InitializeComponent();
	}
	private void Window_Loaded(object sender, RoutedEventArgs e)
	{
		// Перенос вызова команды после полной загрузки окна
		var viewModel = DataContext as AccountViewModel;
		viewModel?.LoadTypeCommand.Execute(null);
	}

	private void Phone_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
	{
		Regex inputRegex = new Regex(@"^[0-9]+$");

		Match match = inputRegex.Match(e.Text);
		if (!match.Success)
		{
			e.Handled = true;
		}
	}

	private void Space_OnPreviewKeyDown(object sender, KeyEventArgs e)
	{
		if (e.Key == Key.Space)
		{
			e.Handled = true;
		}
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

	private void PasswordPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
	{
		if (sender is PasswordBox passwordBox && passwordTextBox != null)
		{
			passwordTextBox.Text = passwordBox.Password;
		}
	}
}