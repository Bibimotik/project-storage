using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;

namespace application.MVVM.View.Auth;
/// <summary>
/// Логика взаимодействия для ConfirmEmailView.xaml
/// </summary>
public partial class ConfirmEmailView : UserControl
{
	public ConfirmEmailView()
	{
		InitializeComponent();
	}
	
	private void TextBox_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
	{
		Regex inputRegex = new Regex(@"^\d*$");
		
		Match match = inputRegex.Match(e.Text);
		if (!match.Success) 
		{
			e.Handled = true;
		}
	}
	
	private void TextBox_OnPreviewKeyDown(object sender, KeyEventArgs e)
	{
		if (e.Key == Key.Space)
		{
			e.Handled = true;
		}
	}

	private void TextBox_OnTextChanged(object sender, TextChangedEventArgs e)
	{
		TextBox textBox = sender as TextBox;
		if (textBox.Text.Length > 0)
		{
			switch (textBox.Name)
			{
				case "_1":
					_2.Focus();
					break;
				case "_2":
					_3.Focus();
					break;
				case "_3":
					_4.Focus();
					break;
				case "_4":
					_5.Focus();
					break;
				case "_5":
					_6.Focus();
					break;
				default:
					break;
			}
		}
	}
}
