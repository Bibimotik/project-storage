using application.MVVM.View.Auth;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace application.MVVM.ViewModel;
internal partial class AuthViewModel : ObservableObject
{
	[ObservableProperty]
	private object? currentView;
	[ObservableProperty]
	private bool authTypeRegistration;
	[ObservableProperty]
	private bool authTypeRegistration2;
	[ObservableProperty]
	private bool authTypeLogin;
	[ObservableProperty]
	private bool authTypeConfirmEmail;
	[ObservableProperty]
	private bool authTypeConfirmEmailReverse;

	[RelayCommand]
	private void Login()
	{
		CurrentView = new LoginView();
		AuthTypeLogin = true;
		AuthTypeRegistration = false;
		AuthTypeRegistration2 = false;
		AuthTypeConfirmEmail = true;
		AuthTypeConfirmEmailReverse = !AuthTypeConfirmEmail;
	}

	[RelayCommand]
	private void Registration1()
	{
		CurrentView = new RegistrationCompanyStage1View();
		AuthTypeLogin = false;
		AuthTypeRegistration = true;
		AuthTypeRegistration2 = false;
		AuthTypeConfirmEmail = true;
		AuthTypeConfirmEmailReverse = !AuthTypeConfirmEmail;
	}

	[RelayCommand]
	private void Registration2()
	{
		CurrentView = new RegistrationCompanyStage2View();
		AuthTypeLogin = false;
		AuthTypeRegistration = false;
		AuthTypeRegistration2 = true;
		AuthTypeConfirmEmail = true;
		AuthTypeConfirmEmailReverse = !AuthTypeConfirmEmail;
	}

	[RelayCommand]
	private void ConfirmEmail()
	{
		CurrentView = new ConfirmPasswordView();
		AuthTypeLogin = false;
		AuthTypeRegistration = false;
		AuthTypeRegistration2 = false;
		AuthTypeConfirmEmail = false;
		AuthTypeConfirmEmailReverse = !AuthTypeConfirmEmail;
	}

	[RelayCommand]
	private void Check()
	{
		// check code in Email confirmation
	}

	// сделать логику для определения куда нужно вернуться из Email confirmation - в Login или Registration2

	public AuthViewModel()
	{
		Login();
	}
}
