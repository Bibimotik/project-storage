using System.Diagnostics;
using System.Reflection;
using System.Windows;

using application.Abstraction;
using application.Abstraction.Interfaces;
using application.MVVM.Model;
using application.MVVM.View.Auth;
using application.MVVM.View.Pages;
using application.MVVM.ViewModel.Auth;
using application.Utilities;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using CSharpFunctionalExtensions;

using Microsoft.Extensions.DependencyInjection;

using static application.Abstraction.EntityAbstraction;

namespace application.MVVM.ViewModel;

public partial class AuthViewModel : ObservableObject
{
	//private readonly IEntityApi _entityApi;
	private readonly IEntityService _entityService;
	private readonly IEntityRepository _entityRepository;
	private readonly IAuthService _authService;
	private readonly INavigationService _navigationService;
	private readonly ISecurityService _securityService;
	private readonly IMailService _mailService;
	private readonly RegistrationUserViewModel _registrationUserViewModel;
	private readonly IParserINNService _parserInnService;
	private readonly ISupportRepository _supportRepository;
	private readonly IServiceProvider _serviceProvider;

	public static event Action<string>? Invalided;

	[ObservableProperty]
	private object? currentView;

	[ObservableProperty]
	private bool authTypeLogin,
		authTypeLoginReverse,
		authTypeRegistration,
		authTypeRegistrationUser,
		authTypeRegistrationUserReverse,
		authTypeRegistrationCompany1,
		authTypeRegistrationCompany2,
		authTypeConfirmEmail,
		authTypeConfirmEmailReverse,
		authSendProblem,
		authApplicationInfo;

	public AuthViewModel(
		//IEntityApi entityApi,
		IEntityRepository entityRepository,
		IEntityService entityService,
		IAuthService authService,
		INavigationService navigationService,
		ISecurityService securityService,
		IMailService mailService,
		RegistrationUserViewModel registrationUserViewModel,
		IParserINNService parserInnService,
		ISupportRepository supportRepository,
		IServiceProvider serviceProvider)
	{
		_entityRepository = entityRepository;
		//_entityApi = entityApi;
		_entityService = entityService;
		_authService = authService;
		_navigationService = navigationService;
		_securityService = securityService;
		_mailService = mailService;
		_registrationUserViewModel = registrationUserViewModel;
		_parserInnService = parserInnService;
		_supportRepository = supportRepository;
		_serviceProvider = serviceProvider;

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
		AuthSendProblem = false;
		AuthApplicationInfo = false;
	}
	[RelayCommand]
	private void RegistrationUser()
	{
		EntityModel model = EntityModel.Model;
		model.EntityType = EntityType.User;

		model.Email = string.Empty;
		model.Message = string.Empty;
		model.Code = string.Empty;
		model.InputCode = string.Empty;

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
		AuthSendProblem = false;
		AuthApplicationInfo = false;
	}
	[RelayCommand]
	private void RegistrationCompany1()
	{
		EntityModel model = EntityModel.Model;
		model.EntityType = EntityType.Company;

		//CurrentView = new RegistrationCompanyStage1View();
		CurrentView = _serviceProvider.GetRequiredService<RegistrationCompanyStage1View>();
		AuthTypeLogin = false;
		AuthTypeLoginReverse = !AuthTypeLogin;
		AuthTypeRegistration = true;
		AuthTypeRegistrationUser = false;
		AuthTypeRegistrationUserReverse = !AuthTypeRegistrationUser;
		AuthTypeRegistrationCompany1 = true;
		AuthTypeRegistrationCompany2 = false;
		AuthTypeConfirmEmail = true;
		AuthTypeConfirmEmailReverse = !AuthTypeConfirmEmail;
		AuthSendProblem = false;
		AuthApplicationInfo = false;
	}
	[RelayCommand]
	private void RegistrationCompany2()
	{
		EntityModel model = EntityModel.Model;

		if (!IsValidModel(model, stage: CompanyRegistrationStages.First))
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
		AuthSendProblem = false;
		AuthApplicationInfo = false;
	}
	[RelayCommand]
	private void Support()
	{
		EntityModel model = EntityModel.Model;
		model.EntityType = EntityType.Support;
		// TODO - я хз как это сделать лучше
		model.FirstName = string.Empty;
		model.SecondName = string.Empty;
		model.ThirdName = string.Empty;
		model.Phone = string.Empty;
		model.INN = string.Empty;
		model.KPP = string.Empty;
		model.FullName = string.Empty;
		model.ShortName = string.Empty;
		model.LegalAddress = string.Empty;
		model.PostalAddress = string.Empty;
		model.OGRN = string.Empty;
		model.Director = string.Empty;
		model.Email = string.Empty;
		model.Password = string.Empty;
		model.ConfirmPassword = string.Empty;
		model.Code = string.Empty;
		model.InputCode = string.Empty;

		CurrentView = _serviceProvider.GetRequiredService<SupportView>();

		AuthTypeLogin = false;
		AuthTypeLoginReverse = false;
		AuthTypeRegistration = false;
		AuthTypeRegistrationUser = false;
		AuthTypeRegistrationUserReverse = false;
		AuthTypeRegistrationCompany1 = false;
		AuthTypeRegistrationCompany2 = false;
		AuthTypeConfirmEmail = false;
		AuthTypeConfirmEmailReverse = false;
		AuthSendProblem = true;
		AuthApplicationInfo = false;
	}
	[RelayCommand]
	private async Task ConfirmEmail()
	{
		if (!await Registration())
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
		AuthSendProblem = false;
		AuthApplicationInfo = false;
	}
	[RelayCommand]
	private void SwitchView()
	{

	}
	[RelayCommand]
	private async Task Check()
	{
		EntityModel model = EntityModel.Model;

		Console.WriteLine("input code: " + model.InputCode);
		Console.WriteLine("storage code: " + model.Code);
		Console.WriteLine("storage code decrypy: " + _securityService.Decrypt(model.Code));

		if (model.InputCode != _securityService.Decrypt(model.Code))
			return;

		Result<Guid> id = new();

		if (model.EntityType == EntityType.User || model.EntityType == EntityType.Company)
		{
			var reg = await _entityService.Registration(model);
			if (reg.IsFailure)
			{
				MessageBox.Show(reg.Error);
				return;
			}
			id = reg.Value.Id;
		}
		else
		{
			return;
		}

		if (id.IsFailure)
		{
			Debug.WriteLine(id.Error);
			return;
		}

		_navigationService.ShowMain();

		Console.WriteLine("ID: " + id.Value.ToString());
	}
	// TODO - почему название ...Button
	[RelayCommand]
	private async Task LoginButton()
	{
		EntityModel model = EntityModel.Model;
		//if (string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.Password))
		//	return;

		if (model.Email == "admin" && model.Password == "Admin123")
		{
			MessageBox.Show("qweqweqwe");

			Debug.WriteLine($"email: {model.Email}");
			Debug.WriteLine($"password: {model.Password}");

			_authService.SaveAuthData(EntityModel.Model.Email, EntityModel.Model.Password);
			_authService.LoadAuthData();

			_navigationService.ShowAdmin();
			return;
		}

		if (!IsValidModel(model, isLogin: true))
			return;

		var user = await _entityService.Login(model.Email, model.Password);
		// TODO - переделать
		if (user.IsFailure)
		{
			if (user.Error == "email")
				Invalided?.Invoke(nameof(EntityModel.Email));
			else if (user.Error == "password")
				Invalided?.Invoke(nameof(EntityModel.Password));

			EntityModel.Reset();
			return;
		}

		Debug.WriteLine($"email: {model.Email}");
		Debug.WriteLine($"password: {model.Password}");

		_authService.SaveAuthData(EntityModel.Model.Email, EntityModel.Model.Password);
		_authService.LoadAuthData();

		_navigationService.ShowMain();
	}
	[RelayCommand]
	public async Task<ParserModel?> GetParserDataINN(string inputINN)
	{
		var (parserData, error) = await _parserInnService.GetParserDataINN(inputINN);

		if (!string.IsNullOrEmpty(error))
		{
			MessageBox.Show(error);
			return null;
		}
		if (parserData != null)
			return parserData;

		return null;
	}
	[RelayCommand]
	private void Info()
	{
		CurrentView = _serviceProvider.GetRequiredService<InfoView>();

		AuthTypeLogin = false;
		AuthTypeLoginReverse = false;
		AuthTypeRegistration = false;
		AuthTypeRegistrationUser = false;
		AuthTypeRegistrationUserReverse = false;
		AuthTypeRegistrationCompany1 = false;
		AuthTypeRegistrationCompany2 = false;
		AuthTypeConfirmEmail = false;
		AuthTypeConfirmEmailReverse = false;
		AuthSendProblem = false;
		AuthApplicationInfo = true;
	}

	private async Task<bool> Registration()
	{
		EntityModel model = EntityModel.Model;

		switch (model.EntityType)
		{
			case EntityType.User:
				if (!IsValidModel(model))
					return false;
				break;
			case EntityType.Company:
				if (!IsValidModel(model, stage: CompanyRegistrationStages.Second))
					return false;
				break;
			case EntityType.Support:
				if (!IsValidModel(model))
					return false;
				break;
			default:
				return false;
		}

		// TODO - как минимум при разном пароле переходит к блоке кода ниже, хотя должен возвращать return;

		if (model.EntityType == EntityType.User || model.EntityType == EntityType.Company)
		{
			var email = await _entityRepository.Get(model.Email);
			if (email is not null)
			{
				MessageBox.Show($"Пользователь с почтой {model.Email} уже существует");
				return false;
			}
		}

		if (model.EntityType == EntityType.User || model.EntityType == EntityType.Company)
		{
			if (!model.Password.Equals(model.ConfirmPassword))
				return false;
		}

		string code = GenerateRandomCode();
		Debug.WriteLine(code);
		string encryptedCode = _securityService.Encrypt(code);
		await _mailService.SendMail(code, model.Email);

		model.Code = encryptedCode;

		return true;
	}

	private bool IsValidModel(EntityModel model, bool isLogin = false, CompanyRegistrationStages stage = CompanyRegistrationStages.First)
	{
		// TODO - а зачем оно ваще?
		_registrationUserViewModel.ClearValidationErrors();

		switch (model.EntityType)
		{
			case EntityType.User:
				IEnumerable<PropertyInfo>? userProperties;
				if (isLogin)
				{
					userProperties = model
						.GetType()
						.GetProperties()
						.Where(p => Attribute.IsDefined(p, typeof(RequiredForLoginAttribute)))
						.Where(p => Attribute.IsDefined(p, typeof(RequiredForValidationAttribute)));
				}
				else
				{
					userProperties = model
						.GetType()
						.GetProperties()
						.Where(p => Attribute.IsDefined(p, typeof(RequiredForUserAttribute)))
						.Where(p => Attribute.IsDefined(p, typeof(RequiredForValidationAttribute)));
				}

				return IsValidModelConditions(userProperties, model);
			case EntityType.Company:
				var companyPropertiesStage1 = model
					.GetType()
					.GetProperties()
					.Where(p => Attribute.IsDefined(p, typeof(RequiredForCompany1Attribute)))
					.Where(p => Attribute.IsDefined(p, typeof(RequiredForValidationAttribute)));

				var companyPropertiesStage2 = model
						.GetType()
						.GetProperties()
						.Where(p => Attribute.IsDefined(p, typeof(RequiredForCompany2Attribute)))
						.Where(p => Attribute.IsDefined(p, typeof(RequiredForValidationAttribute)));

				if (stage == CompanyRegistrationStages.First)
					return IsValidModelConditions(companyPropertiesStage1, model);
				else if (stage == CompanyRegistrationStages.Second)
					return IsValidModelConditions(companyPropertiesStage2, model);
				break;
			case EntityType.Support:
				var supportProperties = model
					.GetType()
					.GetProperties()
					.Where(p => Attribute.IsDefined(p, typeof(RequiredForSupportAttribute)));

				return IsValidModelConditions(supportProperties, model);
			default:
				return false;
		}

		return true;
	}

	private bool IsValidModelConditions(IEnumerable<PropertyInfo> properties, EntityModel model)
	{
		foreach (var property in properties)
		{
			Debug.WriteLine("IsValidModel prop: " + property.Name);

			var value = property.GetValue(model) as string;

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
			if (
				model.EntityType == EntityType.Support &&
				!Enum.TryParse(typeof(SupportProperties), property.Name, out _)
				)
			{
				Debug.WriteLine("support");
				continue;
			}

			Debug.WriteLine("invalid prop:");

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
	
	[RelayCommand]
	private async Task SendSupport()
	{
		EntityModel model = EntityModel.Model;

		Console.WriteLine("input code: " + model.InputCode);
		Console.WriteLine("storage code: " + model.Code);
		Console.WriteLine("storage code decrypy: " + _securityService.Decrypt(model.Code));

		if (model.InputCode != _securityService.Decrypt(model.Code))
			return;

		Result<Guid> id = new();

		/*SupportModel supportModel = new(
			Guid.NewGuid(),
			null,
			null,
			"edjedji",
			null
		);
		*/

		if (id.IsFailure)
		{
			Debug.WriteLine(id.Error);
			return;
		}

		MessageBox.Show("УРА SUPPORT");
	}

	private string GenerateRandomCode()
	{
		Random rand = new();
		return rand.Next(100000, 999999).ToString();
	}
}
