using System.Diagnostics;
using System.Net.Http;
using System.Reflection;
using System.Windows;

using application.Abstraction;
using application.MVVM.Model;
using application.MVVM.View.Auth;
using application.MVVM.View.Pages;
using application.MVVM.ViewModel.Auth;
using application.Services;
using application.Utilities;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using CSharpFunctionalExtensions;

using MailServiceLibrary;

using Microsoft.Extensions.DependencyInjection;

using Newtonsoft.Json;

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
	private readonly IParserINNService _parserInnService;
	private readonly IServiceProvider _serviceProvider;

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
	[ObservableProperty]
	private bool authSendProblem;
	[ObservableProperty]
	private bool authApplicationInfo;

	public AuthViewModel(IEntityRepository entityRepository,
		IAuthService authService,
		INavigationService navigationService,
		ISecurityService securityService,
		IMailService mailService,
		RegistrationUserViewModel registrationUserViewModel,
		IParserINNService parserInnService,
		IServiceProvider serviceProvider)
	{
		_entityRepository = entityRepository;
		_authService = authService;
		_navigationService = navigationService;
		_securityService = securityService;
		_mailService = mailService;
		_registrationUserViewModel = registrationUserViewModel;
		_parserInnService = parserInnService;
		_serviceProvider = serviceProvider;

		Login();
	}
	public AuthViewModel(IParserINNService parserInnService)
	{
		_parserInnService = parserInnService;
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
		AuthSendProblem = false;
		AuthApplicationInfo = false;
	}
	[RelayCommand]
	private void RegistrationCompany2()
	{
		EntityModel model = EntityModel.Model;

		if (!IsValidModel(model, CompanyRegistrationStages.First))
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
			case EntityType.Support:
				await _entityRepository.SendToSupport(model);
				RegistrationUser();
				return;
			default:
				return;
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
	[RelayCommand]
	public async Task<ParserModel> GetParserDataINN(string inputINN)
	{
		var parserData = await _parserInnService.GetParserDataAsync(inputINN);
		if (parserData != null)
		{
			EntityModel.Model ??= new EntityModel();
			EntityModel model = EntityModel.Model;

			model.INN = inputINN;
			model.KPP = parserData.Kpp;
			model.FullName = parserData.FullName;
			model.ShortName = parserData.ShortName;
			model.OGRN = parserData.Ogrn;
			if (parserData.Director != null)
			{
				string cleanedDirector = parserData.Director
					.Replace("ГЕНЕРАЛЬНЫЙ", "")
					.Replace("ДИРЕКТОР", "")
					.Replace(":", "")
					.Trim();

				model.Director = cleanedDirector;
			}
		}
		else
		{
			MessageBox.Show("Не удалось получить данные.");
		}

		return parserData;
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

	private bool Registration()
	{
		EntityModel model = EntityModel.Model;

		switch (model.EntityType)
		{
			case EntityType.User:
				if (!IsValidModel(model))
					return false;
				break;
			case EntityType.Company:
				if (!IsValidModel(model, CompanyRegistrationStages.Second))
					return false;
				break;
			case EntityType.Support:
				if (!IsValidModel(model))
					return false;
				break;
			default:
				return false;
		}

		if (model.EntityType == EntityType.User || model.EntityType == EntityType.Company)
		{
			Result email = _entityRepository.IsEmailExist(model.Email);
			if (email.IsFailure)
			{
				Debug.WriteLine("false");
				MessageBox.Show(email.Error);
				return false;
			}
		}

		if (model.EntityType == EntityType.User || model.EntityType == EntityType.Company)
		{
			if (!model.Password.Equals(model.ConfirmPassword))
				return false;
		}

		string code = GenerateRandomCode();
		Console.WriteLine(code);
		string encryptedCode = _securityService.Encrypt(code);
		_mailService.SendMail(code, model.Email);

		model.Code = encryptedCode;

		return true;
	}

	private bool IsValidModel(EntityModel model, CompanyRegistrationStages stage = CompanyRegistrationStages.First)
	{
		_registrationUserViewModel.ClearValidationErrors();

		var userProperties = model
			.GetType()
			.GetProperties()
			.Where(p => Attribute.IsDefined(p, typeof(RequiredForUserAttribute)))
			.Where(p => Attribute.IsDefined(p, typeof(RequiredForValidationAttribute)));

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

		var supportProperties = model
			.GetType()
			.GetProperties()
			.Where(p => Attribute.IsDefined(p, typeof(RequiredForSupportAttribute)));

		switch (model.EntityType)
		{
			case EntityType.User:
				return IsValidModelConditions(userProperties, model);
			case EntityType.Company:
				if (stage == CompanyRegistrationStages.First)
					return IsValidModelConditions(companyPropertiesStage1, model);
				else if (stage == CompanyRegistrationStages.Second)
					return IsValidModelConditions(companyPropertiesStage2, model);
				break;
			case EntityType.Support:
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


	private string GenerateRandomCode()
	{
		Random rand = new();
		return rand.Next(100000, 999999).ToString();
	}
}
