using System.Windows.Controls;

using application.MVVM.ViewModel.Pages;

namespace application.MVVM.View.Pages;

public partial class AccountView : UserControl
{
	public AccountView(AccountViewModel accountViewModel)
	{
		DataContext = accountViewModel;
		InitializeComponent();
	}
}