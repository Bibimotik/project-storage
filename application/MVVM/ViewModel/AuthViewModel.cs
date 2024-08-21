using System.Diagnostics;
using System.Reflection;
using System.Windows;

using application.Abstraction;
using application.MVVM.Model;
using application.MVVM.View.Auth;
using application.MVVM.ViewModel.Auth;
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
	private readonly RegistrationUserViewModel _registrationUserViewModel;

	public static event Action<string>? Invalided;
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

	public AuthViewModel(IEntityRepository entityRepository,
		IAuthService authService,
		INavigationService navigationService,
		ISecurityService securityService,
		IMailService mailService,
		RegistrationUserViewModel registrationUserViewModel)
	{
		_entityRepository = entityRepository;
		_authService = authService;
		_navigationService = navigationService;
		_securityService = securityService;
		_mailService = mailService;
		_registrationUserViewModel = registrationUserViewModel;

		Login();
	}

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
	}
	[RelayCommand]
	private void RegistrationUser()
	{
		EntityModel model = EntityModel.Model;
		model.EntityType = EntityType.User;

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
		EntityModel model = EntityModel.Model;
		model.EntityType = EntityType.Company;

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
	// TODO - кнопка Next
	[RelayCommand]
	private void RegistrationCompany2()
	{
		EntityModel model = EntityModel.Model;

		if (!IsValidModel(model))
			return;

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

		if (!IsValidModel(model))
			return false;

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
		_mailService.SendMail(code, model.Email);

		model.Code = encryptedCode;

		return true;
	}

	// TODO - с помощью этого метода должны тригеррить поля по кнопке Next, вопрос как
	private bool IsValidModel(EntityModel model)
	{
		_registrationUserViewModel.ClearValidationErrors();

		//var properties = model
		//	.GetType()
		//	.GetProperties()
		//	.Where(p => Attribute.IsDefined(p, typeof(RequiredForValidationAttribute)));

		var userProperties = model
			.GetType()
			.GetProperties()
			.Where(p => Attribute.IsDefined(p, typeof(RequiredForUserAttribute)))
			.Where(p => Attribute.IsDefined(p, typeof(RequiredForValidationAttribute)));

		var companyProperties = model
			.GetType()
			.GetProperties()
			.Where(p => Attribute.IsDefined(p, typeof(RequiredForCompanyAttribute)))
			.Where(p => Attribute.IsDefined(p, typeof(RequiredForValidationAttribute)));

		switch (model.EntityType)
		{
			case EntityType.User:
				return IsValidModelConditions(userProperties, model);
			case EntityType.Company:
				return IsValidModelConditions(companyProperties, model);
			default:
				return false;
		}

		//Debug.WriteLine("user:");
		//foreach (var prop in properties2)
		//{
		//	Debug.WriteLine(prop.Name);
		//}
		//Debug.WriteLine("company:");
		//foreach (var prop in properties3)
		//{
		//	Debug.WriteLine(prop.Name);
		//}

		// TODO - без него метод ругается что нада чета возвращать, хотя switch все равно чета вернет
		return true;
	}

	private bool IsValidModelConditions(IEnumerable<PropertyInfo> properties, EntityModel model)
	{
		foreach (var property in properties)
		{
			Debug.WriteLine("IsValidModel prop: " + property.Name);

			var value = property.GetValue(model) as string;
			// TODO -  эта проверка чисто чтобы была для отладки, можно убрать в будущем
			if (!string.IsNullOrWhiteSpace(value))
			{
				Debug.WriteLine("value: " + value);
				continue;
			}

			if (
				model.EntityType == EntityType.User &&
				!Enum.TryParse(typeof(UserProperties), property.Name, out _)
				)
			{
				Debug.WriteLine("user");
				continue;
			}
			if (
				model.EntityType == EntityType.Company &&
				!Enum.TryParse(typeof(CompanyProperties), property.Name, out _)
				)
			{
				Debug.WriteLine("company");
				continue;
			}

			if (string.IsNullOrWhiteSpace(value))
			{
				Debug.WriteLine("prop in if " + property.Name);
				Invalided?.Invoke(property.Name);
				return false;
			}
			else
			{
				return false;
			}
		}

		return true;
	}

	//private bool IsEntityProperty(string propertyName)
	//{
	//	return Enum.TryParse(typeof(UserProperties), propertyName, out _);
	//}

	private string GenerateRandomCode()
	{
		Random rand = new();
		return rand.Next(100000, 999999).ToString();
	}
}
