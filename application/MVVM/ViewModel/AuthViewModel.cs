using System.Collections.ObjectModel;
using System.Diagnostics;

using application.Abstraction;
using application.MVVM.View.Auth;
using application.Services;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace application.MVVM.ViewModel;

public partial class AuthViewModel : ObservableObject
{
	private readonly IStatusRepository _statusRepository;
	private readonly IAuthService _authService;

	[ObservableProperty]
	private object? currentView;
	[ObservableProperty]
	private bool authTypeLogin;
	[ObservableProperty]
	private bool authTypeLoginReverse;
	[ObservableProperty]
	private bool authTypeRegistration;
	[ObservableProperty]
	private bool authTypeRegistrationUser;
	[ObservableProperty]
	private bool authTypeRegistrationUserReverse;
	[ObservableProperty]
	private bool authTypeRegistrationCompany1;
	[ObservableProperty]
	private bool authTypeRegistrationCompany2;
	[ObservableProperty]
	private bool authTypeConfirmEmail;
	[ObservableProperty]
	private bool authTypeConfirmEmailReverse;

	public AuthViewModel(IStatusRepository statusRepository, IAuthService authService)
	{
		_statusRepository = statusRepository;
		_authService = authService;
		Login();
	}

	// TODO - возможно можно как то переделать логику на switch case
	[RelayCommand]
	private void Login()
	{
		CurrentView = new LoginView();
		AuthTypeLogin = true;
		AuthTypeLoginReverse = !AuthTypeLogin;
		AuthTypeRegistration = false;
		AuthTypeRegistrationUser = false;
		AuthTypeRegistrationUserReverse = !AuthTypeRegistrationUser;
		AuthTypeRegistrationCompany1 = false;
		AuthTypeRegistrationCompany2 = false;
		AuthTypeConfirmEmail = true;
		AuthTypeConfirmEmailReverse = !AuthTypeConfirmEmail;

		// TODO - проверка в бд
		ObservableCollection<StatusModel> status = new(_statusRepository.GetAllStatus());

		foreach (StatusModel statusModel in status)
			Debug.WriteLine(statusModel.Title);

		//_authService.SaveAuthData("email@gmail.com", "qwe123");
		_authService.LoadAuthData();
}
	[RelayCommand]
	private void RegistrationUser()
	{
		CurrentView = new RegistrationUserView();
		AuthTypeLogin = false;
		AuthTypeLoginReverse = !AuthTypeLogin;
		AuthTypeRegistration = true;
		AuthTypeRegistrationUser = true;
		AuthTypeRegistrationUserReverse = !AuthTypeRegistrationUser;
		AuthTypeRegistrationCompany1 = false;
		AuthTypeRegistrationCompany2 = false;
		AuthTypeConfirmEmail = true;
		AuthTypeConfirmEmailReverse = !AuthTypeConfirmEmail;
	}
	[RelayCommand]
	private void RegistrationCompany1()
	{
		CurrentView = new RegistrationCompanyStage1View();
		AuthTypeLogin = false;
		AuthTypeLoginReverse = !AuthTypeLogin;
		AuthTypeRegistration = true;
		AuthTypeRegistrationUser = false;
		AuthTypeRegistrationUserReverse = !AuthTypeRegistrationUser;
		AuthTypeRegistrationCompany1 = true;
		AuthTypeRegistrationCompany2 = false;
		AuthTypeConfirmEmail = true;
		AuthTypeConfirmEmailReverse = !AuthTypeConfirmEmail;
	}
	[RelayCommand]
	private void RegistrationCompany2()
	{
		CurrentView = new RegistrationCompanyStage2View();
		AuthTypeLogin = false;
		AuthTypeLoginReverse = !AuthTypeLogin;
		AuthTypeRegistration = true;
		AuthTypeRegistrationUser = false;
		AuthTypeRegistrationUserReverse = !AuthTypeRegistrationUser;
		AuthTypeRegistrationCompany1 = false;
		AuthTypeRegistrationCompany2 = true;
		AuthTypeConfirmEmail = true;
		AuthTypeConfirmEmailReverse = !AuthTypeConfirmEmail;
	}
	[RelayCommand]
	private void ConfirmEmail()
	{
		CurrentView = new ConfirmEmailView();
		AuthTypeLogin = false;
		AuthTypeLoginReverse = !AuthTypeLogin;
		AuthTypeRegistration = false;
		AuthTypeRegistrationUser = false;
		AuthTypeRegistrationUserReverse = !AuthTypeRegistrationUser;
		AuthTypeRegistrationCompany1 = false;
		AuthTypeRegistrationCompany2 = false;
		AuthTypeConfirmEmail = false;
		AuthTypeConfirmEmailReverse = !AuthTypeConfirmEmail;
	}
	[RelayCommand]
	private void Check()
	{
		// TODO - код проверки почты
	}
}
