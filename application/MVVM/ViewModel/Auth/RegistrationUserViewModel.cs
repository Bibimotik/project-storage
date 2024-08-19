using System.Diagnostics;
using System.Text.RegularExpressions;

using application.MVVM.Model;

using CommunityToolkit.Mvvm.ComponentModel;

using static application.Abstraction.EntityAbstraction;

namespace application.MVVM.ViewModel.Auth;

public partial class RegistrationUserViewModel : ObservableObject
{
	private readonly Dictionary<string, Action<string?>> _validationActions;

	[ObservableProperty]
	private string firstName = string.Empty;
	[ObservableProperty]
	private string secondName = string.Empty;
	[ObservableProperty]
	private string thirdName = string.Empty;
	[ObservableProperty]
	private string phone = string.Empty;
	[ObservableProperty]
	private string email = string.Empty;
	[ObservableProperty]
	private string password = string.Empty;
	[ObservableProperty]
	private string confirmPassword = string.Empty;

	[ObservableProperty]
	private bool isInvalidFirstName = false;
	[ObservableProperty]
	private bool isInvalidSecondName = false;
	[ObservableProperty]
	private bool isInvalidThirdName = false;
	[ObservableProperty]
	private bool isInvalidEmail = false;
	[ObservableProperty]
	private bool isInvalidPhone = false;
	[ObservableProperty]
	private bool isInvalidPassword = false;
	[ObservableProperty]
	private bool isInvalidConfirmPassword = false;
	[ObservableProperty]
	private bool isPasswordFormatInvalid = false;
	[ObservableProperty]
	private bool arePasswordsMismatch = false;

	public RegistrationUserViewModel()
	{
		AuthViewModel.Invalided += OnInvalided;

		_validationActions = new Dictionary<string, Action<string?>>
		{
			{ nameof(FirstName), value => IsInvalidFirstName = ValidateAndCreateModel(value) },
			{ nameof(SecondName), value => IsInvalidSecondName = ValidateAndCreateModel(value) },
			{ nameof(ThirdName), value => IsInvalidThirdName = ValidateAndCreateModel(value) },
			{ nameof(Phone), value => IsInvalidPhone = ValidateAndCreateModel(value) },
			{ nameof(Email), value => IsInvalidEmail = ValidateAndCreateModel(value) },
			{ nameof(Password), value => IsInvalidPassword = ValidateAndCreateModel(value) },
			{ nameof(ConfirmPassword), value => IsInvalidConfirmPassword = ValidateAndCreateModel(value) }
		};
	}

	partial void OnFirstNameChanged(string value) => IsInvalidFirstName = ValidateAndCreateModel(value);
	partial void OnSecondNameChanged(string value) => IsInvalidSecondName = ValidateAndCreateModel(value);
	partial void OnThirdNameChanged(string value) => IsInvalidThirdName = ValidateAndCreateModel(value);
	partial void OnPhoneChanged(string value) => IsInvalidPhone = ValidateAndCreateModel(value);
	partial void OnEmailChanged(string value)
	{
		try
		{
			Debug.WriteLine("email " + value);
			var regex = new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

			if (!regex.IsMatch(value))
			{
				IsInvalidEmail = true;
				return;
			}

			IsInvalidEmail = false;
		}
		catch (RegexMatchTimeoutException)
		{
			IsInvalidEmail = false;
			return;
		}

		IsInvalidEmail = ValidateAndCreateModel(value);
	}
	partial void OnPasswordChanged(string value)
	{
		try
		{
			var regex = new Regex("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d).{8,}$", RegexOptions.Compiled);

			if (!regex.IsMatch(value))
			{
				IsInvalidPassword = true;
				ArePasswordsMismatch = false;
				return;
			}

			IsInvalidPassword = false;

			if (!EntityModel.ComparePasswords(value, ConfirmPassword))
			{
				ArePasswordsMismatch = true;
				return;
			}

			ArePasswordsMismatch = false;
		}
		catch (RegexMatchTimeoutException)
		{
			IsInvalidPassword = false;
			ArePasswordsMismatch = false;
			return;
		}

		IsInvalidPassword = ValidateAndCreateModel(value);
	}
	partial void OnConfirmPasswordChanged(string value)
	{
		try
		{
			var regex = new Regex("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d).{8,}$", RegexOptions.Compiled);

			if (!regex.IsMatch(value))
			{
				IsPasswordFormatInvalid = true;
				ArePasswordsMismatch = false;
				return;
			}

			IsPasswordFormatInvalid = false;

			if (!EntityModel.ComparePasswords(Password, value))
			{
				ArePasswordsMismatch = true;
				return;
			}

			ArePasswordsMismatch = false;
		}
		catch (RegexMatchTimeoutException)
		{
			IsPasswordFormatInvalid = false;
			ArePasswordsMismatch = false;
			return;
		}

		CreateModel();
	}

	private void OnInvalided(string property)
	{
		if (_validationActions.TryGetValue(property, out var validate))
		{
			validate(string.Empty);
		}
	}

	private bool ValidateAndCreateModel(string? value)
	{
		CreateModel();
		return string.IsNullOrWhiteSpace(value);
	}

	public void ClearValidationErrors()
	{
		IsInvalidFirstName = false;
		IsInvalidSecondName = false;
		IsInvalidThirdName = false;
		IsInvalidEmail = false;
		IsInvalidPhone = false;
		IsInvalidPassword = false;
		IsInvalidConfirmPassword = false;
		IsPasswordFormatInvalid = false;
		ArePasswordsMismatch = false;
	}

	private void CreateModel()
	{
		EntityModel.Model ??= new EntityModel();

		EntityModel model = EntityModel.Model;
		model.Id = Guid.NewGuid();
		model.EntityType = EntityType.User;
		model.FirstName = FirstName;
		model.SecondName = SecondName;
		model.ThirdName = ThirdName;
		model.Phone = Phone;
		model.Email = Email;
		model.Password = Password;
		model.ConfirmPassword = ConfirmPassword;
	}

	//partial void OnFirstNameChanged(string value)
	//{
	//	ValidateAndCreateModel(nameof(IsInvalidFirstName), value);


	//	//if (!EntityModel.IsFilledFirstName(value))
	//	//{
	//	//	IsInvalidFirstName = true;
	//	//	return;
	//	//}
	//	// TODO - лично я не вижу смысла в методе из модели, как в блоке выше
	//	//if (string.IsNullOrWhiteSpace(value))
	//	//{
	//	//	IsInvalidFirstName = true;
	//	//	return;
	//	//}

	//	//IsInvalidFirstName = false;
	//	//CreateModel();

	//	// ------- TODO - может так сократить можно

	//	//IsInvalidFirstName = string.IsNullOrWhiteSpace(value);
	//	//if (IsInvalidFirstName)
	//	//	return;

	//	//CreateModel();
	//}
	//partial void OnSecondNameChanged(string value)
	//{
	//	ValidateAndCreateModel(IsInvalidSecondName, value);


	//	//if (!EntityModel.IsFilledSecondName(value))
	//	//{
	//	//	IsInvalidSecondName = true;
	//	//	return;
	//	//}

	//	//IsInvalidSecondName = false;
	//	//CreateModel();
	//}
	//partial void OnThirdNameChanged(string value)
	//{
	//	ValidateAndCreateModel(IsInvalidThirdName, value);


	//	//if (!EntityModel.IsFilledThirdName(value))
	//	//{
	//	//	IsInvalidThirdName = true;
	//	//	return;
	//	//}

	//	//IsInvalidThirdName = false;
	//	//CreateModel();
	//}
	//partial void OnPhoneChanged(string value)
	//{
	//	ValidateAndCreateModel(IsInvalidPhone, value);


	//	//if (!EntityModel.IsFilledPhone(value))
	//	//{
	//	//	IsInvalidPhone = true;
	//	//	return;
	//	//}

	//	//IsInvalidPhone = false;
	//	//CreateModel();
	//}
	//partial void OnEmailChanged(string value)
	//{
	//	ValidateAndCreateModel(IsInvalidEmail, value);


	//	//if (!EntityModel.IsValidEmail(value))
	//	//{
	//	//	IsInvalidEmail = true;
	//	//	return;
	//	//}

	//	//IsInvalidEmail = false;
	//	//CreateModel();
	//}
	//partial void OnPasswordChanged(string value)
	//{
	//	ValidateAndCreateModel(IsInvalidPassword, value);


	//	//if (!EntityModel.IsFilledPassword(value))
	//	//{
	//	//	IsInvalidPassword = true;
	//	//	return;
	//	//}

	//	//IsInvalidPassword = false;
	//	//CreateModel();
	//}
	//partial void OnConfirmPasswordChanged(string value)
	//{
	//	IsPasswordFormatInvalid = false;
	//	ArePasswordsMismatch = false;

	//	if (!EntityModel.IsFilledConfirmPassword(value))
	//	{
	//		IsPasswordFormatInvalid = true;
	//	}
	//	else if (!EntityModel.ComparePasswords(Password, value))
	//	{
	//		ArePasswordsMismatch = true;
	//	}
	//	else
	//	{
	//		CreateModel();
	//	}
	//}
	//private void ValidateAndCreateModel(bool isInvalidProperty, string? value = null)
	//{
	//	isInvalidProperty = string.IsNullOrWhiteSpace(value);
	//	if (isInvalidProperty)
	//	{
	//		Debug.WriteLine("isInvalidProperty " + isInvalidProperty);
	//		return;
	//	}	

	//	CreateModel();
	//}

	//private void OnInvalided(string property)
	//{
	//	Debug.WriteLine("Invalided " + property);

	//	switch (property)
	//	{
	//		case nameof(EntityModel.Email):
	//			ValidateAndCreateModel(IsInvalidEmail);
	//			break;
	//		case nameof(EntityModel.Password):
	//			ValidateAndCreateModel(IsInvalidPassword);
	//			break;
	//		case nameof(EntityModel.ConfirmPassword):
	//			// TODO - и так далее ValidateAndCreateModel(IsInvalid...)
	//			break;
	//		case nameof(EntityModel.FirstName):
	//			break;
	//		case nameof(EntityModel.SecondName):
	//			break;
	//		case nameof(EntityModel.ThirdName):
	//			break;
	//		case nameof(EntityModel.Phone):
	//			break;
	//		case nameof(EntityModel.INN):
	//			break;
	//		case nameof(EntityModel.KPP):
	//			break;
	//		case nameof(EntityModel.FullName):
	//			break;
	//		case nameof(EntityModel.ShortName):
	//			break;
	//		case nameof(EntityModel.LegalAddress):
	//			break;
	//		case nameof(EntityModel.PostalAddress):
	//			break;
	//		case nameof(EntityModel.OGRN):
	//			break;
	//		case nameof(EntityModel.Director):
	//			break;
	//		default:
	//			break;
	//	}
	//}
}
