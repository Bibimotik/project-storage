using System.Diagnostics;
using System.Windows;

using application.Abstraction;
using application.MVVM.Model;
using application.MVVM.View.Auth;
using application.Repository;
using application.Services;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using StackExchange.Redis;

namespace application.MVVM.ViewModel;

public partial class AuthViewModel : ObservableObject
{
	private readonly IEntityRepository _entityRepository;
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


	public AuthViewModel(IEntityRepository entityRepository, IAuthService authService, INavigationService navigationService)
	{
		_entityRepository = entityRepository;
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
	// TODO - оставить async void или сделать async Task. Т.к. void отвечает за обработчики событий
	[RelayCommand]
	private async void LoginButton()
	{
		EntityModel model = EntityModel.Model;
		if (string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.Password))
			return;

		EntityModel user = await _entityRepository.GetEntityLogin(model.Email);

		if (user.Email != model.Email || user.Password != model.Password)
			return;

		Debug.WriteLine($"email: {user.Email}");
		Debug.WriteLine($"password: {user.Password}");

		_authService.SaveAuthData(EntityModel.Model.Email, EntityModel.Model.Password);
		_authService.LoadAuthData();

		_navigationService.ShowMain();
	}
	
	private string GenerateRandomCode()
	{
		Random rand = new Random();
		return rand.Next(100000, 999999).ToString();
	}
	
	[RelayCommand]
	private async void UserRegistrationButton()
	{
		EntityModel model = EntityModel.Model;
		model.Id = Guid.NewGuid();
		model.FirstName = "w";
		model.SecondName = "w";
		model.ThirdName = "w";
		model.Phone = "+431463";
		model.Email = "misha2005.b@yandex.ru";
		model.Password = "w";
		model.ConfirmPassword = "w";
		// TODO - сделать проверки на наличие всех заполненных полей

		if (!model.Password.Equals(model.ConfirmPassword))
			return;

		var id = await _entityRepository.UserRegistration(model);

		if (id.IsFailure)
			return;

		Console.WriteLine("ID: " + id.Value.ToString());
		
		ISecurityService securityService = new SecurityService();
		securityService.GenerateKeys();
		string encryptedCode = securityService.Encrypt(GenerateRandomCode());
		
		IMailService mailService = new MailService();
		//mailService.SendMail(securityService.Decrypt(encryptedCode), model.Email); // Используем userEmail, который был установлен при изменении эл. почты

		model.Code = encryptedCode;
	}
}
