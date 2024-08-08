using System.Diagnostics;

using application.Abstraction;
using application.MVVM.Model;
using application.MVVM.View.Auth;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace application.MVVM.ViewModel;

public partial class AuthViewModel : ObservableObject
{
	private readonly IUserRepository _userRepository;
	private readonly IAuthService _authService;
	private readonly INavigationService _navigationService;

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


	public AuthViewModel(IUserRepository userRepository, IAuthService authService, INavigationService navigationService)
	{
		_userRepository = userRepository;
		_authService = authService;
		_navigationService = navigationService;
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
		//ObservableCollection<StatusEntity> status = new(_statusRepository.GetAllStatus());

		//foreach (StatusEntity statusModel in status)
		//	Debug.WriteLine(statusModel.Title);
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
	// TODO - на одинаковые ConfirmEmail можно добавить CommandParameter чтобы различать разные функции регистрации
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

	// TODO - здесь проверка данных из репозитория
	[RelayCommand]
	private void LoginButton()
	{
		LoginModel model = LoginModel.Model;
		if (string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.Password))
			return;

		LoginModel user = _userRepository.GetUserLogin(model.Email);

		if (user.Email != model.Email || user.Password != model.Password)
			return;

		Debug.WriteLine($"email: {user.Email}");
		Debug.WriteLine($"password: {user.Password}");

		_authService.SaveAuthData(LoginModel.Model.Email, LoginModel.Model.Password);
		_authService.LoadAuthData();

		_navigationService.ShowMain();
	}
}
