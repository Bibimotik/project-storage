using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;

using application.MVVM.ViewModel.Pages;

namespace application.MVVM.View.Pages;

public partial class SupportMainView : UserControl
{
	public SupportMainView(SupportMainViewModel mainViewModel)
	{
		DataContext = mainViewModel;
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
    
	private void Space_OnPreviewKeyDown(object sender, KeyEventArgs e)
	{
		if (e.Key == Key.Space)
		{
			e.Handled = true;
		}
	}
}