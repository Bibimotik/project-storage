	using System.Diagnostics;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

using application.MVVM.Model;
using application.Services;

using CommunityToolkit.Mvvm.ComponentModel;

namespace application.MVVM.ViewModel.Auth;

partial class LoginViewModel : ObservableObject
{
	private readonly Dictionary<string, Action<string?>> _validationActions;
	private readonly bool _isInitializing = false;

	[ObservableProperty]
	private string email = string.Empty;
	[ObservableProperty]
	private string password = string.Empty;

	[ObservableProperty]
	private bool isInvalidEmail = false;
	[ObservableProperty]
	private bool isInvalidPassword = false;

	public LoginViewModel()
	{
		_isInitializing = true;

		AuthViewModel.Invalided += OnInvalided;

		_validationActions = new Dictionary<string, Action<string?>>
		{
			{ nameof(EntityModel.Email), value => IsInvalidEmail = ValidateAndCreateModel(value) },
			{ nameof(EntityModel.Password), value => IsInvalidPassword = ValidateAndCreateModel(value) }
		};

		EntityModel.Model ??= new EntityModel();

		EntityModel model = EntityModel.Model;
		Email = model.Email;
		Password = model.Password;

		_isInitializing = false;
	}
	//partial void OnEmailChanged(string value)
	//{
	//	try
	//	{
	//		var regex = new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

	//		if (!regex.IsMatch(value))
	//		{
	//			IsInvalidEmail = true;
	//			return;
	//		}

	//		IsInvalidEmail = false;
	//	}
	//	catch (RegexMatchTimeoutException)
	//	{
	//		IsInvalidEmail = false;
	//		return;
	//	}

	//	IsInvalidEmail = ValidateAndCreateModel(value);
	//}
	partial void OnEmailChanged(string value)
	{
		Debug.WriteLine("email: " + value);
		if (!EntityModel.IsValidEmail(value))
		{
			IsInvalidEmail = ValidateAndCreateModel(value);

			return;
		}
		IsInvalidEmail = false;
	}
	partial void OnPasswordChanged(string value)
	{
		try
		{
			var regex = new Regex("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d).{8,}$", RegexOptions.Compiled);

			if (!regex.IsMatch(value))
			{
				IsInvalidPassword = true;
				return;
			}

			IsInvalidPassword = false;
		}
		catch (RegexMatchTimeoutException)
		{
			IsInvalidPassword = false;
			return;
		}

		IsInvalidPassword = ValidateAndCreateModel(value);
	}

	private bool ValidateAndCreateModel(string? value)
	{
		if (_isInitializing)
			return false;

		CreateModel();
		return string.IsNullOrWhiteSpace(value);
	}

	private void OnInvalided(string property)
	{
		Debug.WriteLine("LOGIN invalided " + property);
		if (_validationActions.TryGetValue(property, out var validate))
		{
			validate(string.Empty);
		}
	}

	private void CreateModel()
	{
		EntityModel.Model ??= new EntityModel();

		EntityModel model = EntityModel.Model;
		model.Email = Email;
		model.Password = Password;

		Debug.WriteLine($"email1: {model.Email}\n" +
			$"password1: {model.Password}");
	}
}
