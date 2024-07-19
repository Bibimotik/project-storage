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
	private bool authTypeLogin;

	[RelayCommand]
	private void Login()
	{
		CurrentView = new LoginView();
		AuthTypeLogin = true;
		AuthTypeRegistration = false;
	}

	[RelayCommand]
	private void Registration1()
	{
		CurrentView = new RegistrationStage1View();
		AuthTypeLogin = false;
		AuthTypeRegistration = true;
	}

	public AuthViewModel()
	{
		Login();
	}
}
