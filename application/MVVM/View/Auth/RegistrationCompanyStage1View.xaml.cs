using System.Windows.Controls;

using application.MVVM.ViewModel.Auth;

namespace application.MVVM.View.Auth;

public partial class RegistrationCompanyStage1View : UserControl
{
	public RegistrationCompanyStage1View(RegistrationCompanyStage1ViewModel registrationViewModel)
	{
		DataContext = registrationViewModel;
		InitializeComponent();
	}
}