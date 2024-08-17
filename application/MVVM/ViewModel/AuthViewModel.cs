using System.Diagnostics;
using System.Windows;

using application.Abstraction;
using application.MVVM.Model;
using application.MVVM.View.Auth;
using application.Utilities;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using CSharpFunctionalExtensions;

using MailServiceLibrary;

using static application.Abstraction.EntityAbstraction;

namespace application.MVVM.ViewModel;

public partial class AuthViewModel : ObservableObject
{
	private readonly IEntityRepository _entityRepository;
	private readonly IAuthService _authService;
	private readonly INavigationService _navigationService;
	private readonly ISecurityService _securityService;
	private readonly IMailService _mailService;

	public static event Action<string> Invalided;
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


	public AuthViewModel(IEntityRepository entityRepository, IAuthService authService, INavigationService navigationService, ISecurityService securityService, IMailService mailService)
	{
		_entityRepository = entityRepository;
		_authService = authService;
		_navigationService = navigationService;
		_securityService = securityService;
		_mailService = mailService;

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
		if (!Registration())
			return;

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
	private void SwitchView()
	{

	}
	[RelayCommand]
	private async Task Check()
	{
		EntityModel model = EntityModel.Model;

		Debug.WriteLine("input code: " + model.InputCode);
		Debug.WriteLine("storage code: " + model.Code);
		Debug.WriteLine("storage code decrypy: " + _securityService.Decrypt(model.Code));

		if (model.InputCode != _securityService.Decrypt(model.Code))
			return;

		Result<Guid> id = new();
		switch (model.EntityType)
		{
			case EntityType.User:
				id = await _entityRepository.UserRegistration(model);
				break;
			case EntityType.Company:
				id = await _entityRepository.UserRegistration(model);
				break;
		}

		if (id.IsFailure)
		{
			Debug.WriteLine(id.Error);
			return;
		}

		MessageBox.Show("УРА");
		_navigationService.ShowMain();

		Console.WriteLine("ID: " + id.Value.ToString());
	}
	[RelayCommand]
	private async Task LoginButton()
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

	private bool Registration()
	{
		EntityModel model = EntityModel.Model;
		//model.Email = "kuncovs1.0@gmail.com";
		//model.Email = "misha2005.b@yandex.ru";

		// TODO - сделать проверки на наличие всех заполненных полей

		if (!IsValidModel(model))
			return false;

		//if (string.IsNullOrWhiteSpace(model.Email))
		//	Invalided?.Invoke();
		//if (string.IsNullOrWhiteSpace(model.Password))
		//	Invalided?.Invoke();
		//if (string.IsNullOrWhiteSpace(model.ConfirmPassword))
		//	Invalided?.Invoke();
		//if (string.IsNullOrWhiteSpace(model.FirstName))
		//	Invalided?.Invoke();
		//if (string.IsNullOrWhiteSpace(model.SecondName))
		//	Invalided?.Invoke();
		//if (string.IsNullOrWhiteSpace(model.ThirdName))
		//	Invalided?.Invoke();
		//if (string.IsNullOrWhiteSpace(model.Phone))
		//	Invalided?.Invoke();
		//if (string.IsNullOrWhiteSpace(model.INN))
		//	Invalided?.Invoke();
		//if (string.IsNullOrWhiteSpace(model.KPP))
		//	Invalided?.Invoke();
		//if (string.IsNullOrWhiteSpace(model.FullName))
		//	Invalided?.Invoke();
		//if (string.IsNullOrWhiteSpace(model.ShortName))
		//	Invalided?.Invoke();
		//if (string.IsNullOrWhiteSpace(model.LegalAddress))
		//	Invalided?.Invoke();
		//if (string.IsNullOrWhiteSpace(model.PostalAddress))
		//	Invalided?.Invoke();
		//if (string.IsNullOrWhiteSpace(model.OGRN))
		//	Invalided?.Invoke();
		//if (string.IsNullOrWhiteSpace(model.Director))
		//	Invalided?.Invoke();

		Result email = _entityRepository.IsEmailExist(model.Email);
		if (email.IsFailure)
		{
			Debug.WriteLine("false");
			MessageBox.Show(email.Error);
			return false;
		}

		if (!model.Password.Equals(model.ConfirmPassword))
			return false;

		string code = GenerateRandomCode();
		string encryptedCode = _securityService.Encrypt(code);
		// TODO - нахуя сразу шфировать а потом в mailService передавать расшифровку
		//_mailService.SendMail(_securityService.Decrypt(encryptedCode), model.Email); // Используем userEmail, который был установлен при изменении эл. почты
		// почему не так
		_mailService.SendMail(code, model.Email);

		model.Code = encryptedCode;

		return true;
	}

	// TODO - вопрос только как это упорядочить чтобы ошибка указывалась на нужный инпут в правильной последовательности
	private bool IsValidModel(EntityModel model)
	{
		var properties = model.GetType().GetProperties()
		.Where(p => Attribute.IsDefined(p, typeof(RequiredForValidationAttribute)));

		foreach (var property in properties)
		{
			var value = property.GetValue(model) as string;
			if (string.IsNullOrWhiteSpace(value))
			{
				Invalided?.Invoke(property.Name);
				return false;
			}
		}
		return true;
	}


	private string GenerateRandomCode()
	{
		Random rand = new();
		return rand.Next(100000, 999999).ToString();
	}
}
