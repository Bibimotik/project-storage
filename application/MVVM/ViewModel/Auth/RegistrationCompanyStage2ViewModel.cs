﻿using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

using application.MVVM.Model;

using CommunityToolkit.Mvvm.ComponentModel;

using static application.Abstraction.EntityAbstraction;

namespace application.MVVM.ViewModel.Auth;

partial class RegistrationCompanyStage2ViewModel : ObservableObject
{
	private readonly Dictionary<string, Action<string?>> _validationActions;
	private readonly bool _isInitializing = false;

	[ObservableProperty]
	private string director = string.Empty;
	[ObservableProperty]
	private string email = string.Empty;
	[ObservableProperty]
	private string password = string.Empty;
	[ObservableProperty]
	private string confirmPassword = string.Empty;

	[ObservableProperty]
	private bool isInvalidDirector = false;
	[ObservableProperty]
	private bool isInvalidEmail = false;
	[ObservableProperty]
	private bool isInvalidPassword = false;
	[ObservableProperty]
	private bool isInvalidConfirmPassword = false;
	[ObservableProperty]
	private bool isPasswordFormatInvalid = false;
	[ObservableProperty]
	private bool arePasswordsMismatch = false;

	// TODO - почему то именно ConfirmPassword копируется в Password и чета там не так. в RegUserVM тоже самое
	public RegistrationCompanyStage2ViewModel()
	{
		_isInitializing = true;

		AuthViewModel.Invalided += OnInvalided;

		_validationActions = new Dictionary<string, Action<string?>>
		{
			{ nameof(EntityModel.Director), value => IsInvalidDirector = ValidateAndCreateModel(value) },
			{ nameof(EntityModel.Email), value => IsInvalidEmail = ValidateAndCreateModel(value) },
			{ nameof(EntityModel.Password), value => IsInvalidPassword = ValidateAndCreateModel(value) },
			{ nameof(EntityModel.ConfirmPassword), value => IsInvalidConfirmPassword = ValidateAndCreateModel(value) }
		};

		EntityModel.Model ??= new EntityModel();

		EntityModel model = EntityModel.Model;
		Director = model.Director;
		Email = model.Email;
		Password = model.Password;
		ConfirmPassword = model.ConfirmPassword;

		_isInitializing = false;
	}

	partial void OnDirectorChanged(string value) => IsInvalidDirector = ValidateAndCreateModel(value);
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

			if (!string.Equals(value, ConfirmPassword))
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

			if (!string.Equals(value, Password))
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

	private bool ValidateAndCreateModel(string? value)
	{
		if (_isInitializing)
			return false;

		CreateModel();
		return string.IsNullOrWhiteSpace(value);
	}

	private void OnInvalided(string property)
	{
		if (_validationActions.TryGetValue(property, out var validate))
		{
			validate(string.Empty);
		}
	}

	private void CreateModel()
	{
		EntityModel.Model ??= new EntityModel();

		EntityModel model = EntityModel.Model;
		model.EntityType = EntityType.Company;
		model.Director = Director;
		model.Email = Email;
		model.Password = Password;
		model.ConfirmPassword = ConfirmPassword;
	}
}
