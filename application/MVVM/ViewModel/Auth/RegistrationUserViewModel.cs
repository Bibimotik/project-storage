using System.Windows;

using application.MVVM.Model;

using CommunityToolkit.Mvvm.ComponentModel;

using static application.Abstraction.EntityAbstraction;

namespace application.MVVM.ViewModel.Auth;

partial class RegistrationUserViewModel : ObservableObject
{
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
	private bool isInvalidEmail = false;
	[ObservableProperty]
	private bool isInvalidFirstName = false;
	[ObservableProperty]
	private bool isInvalidSecondName = false;
	[ObservableProperty]
	private bool isInvalidThirdName = false;
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

	partial void OnFirstNameChanged(string value)
	{
		if (!EntityModel.IsFilledFirstName(value))
		{
			IsInvalidFirstName = true;
			return;
		}
		
		IsInvalidFirstName = false;
		CreateModel();
	}

	partial void OnSecondNameChanged(string value)
	{
		if (!EntityModel.IsFilledSecondName(value))
		{
			IsInvalidSecondName = true;
			return;
		}
		
		IsInvalidSecondName = false;
		CreateModel();
	}
	partial void OnThirdNameChanged(string value)
	{
		if (!EntityModel.IsFilledThirdName(value))
		{
			IsInvalidThirdName = true;
			return;
		}
		
		IsInvalidThirdName = false;
		CreateModel();
	}
	partial void OnPhoneChanged(string value)
	{
		if (!EntityModel.IsFilledPhone(value))
		{
			IsInvalidPhone = true;
			return;
		}
		
		IsInvalidPhone = false;
		CreateModel();
	}
	partial void OnEmailChanged(string value)
	{
		if (!EntityModel.IsValidEmail(value))
		{
			IsInvalidEmail = true;
			return;
		}
		
		IsInvalidEmail = false;
		CreateModel();
	}
	partial void OnPasswordChanged(string value)
	{
		if (!EntityModel.IsFilledPassword(value))
		{
			IsInvalidPassword = true;
			return;
		}
		
		IsInvalidPassword = false;
		CreateModel();
	}
	partial void OnConfirmPasswordChanged(string value)
	{
		IsPasswordFormatInvalid = false;
		ArePasswordsMismatch = false;

		if (!EntityModel.IsFilledConfirmPassword(value))
		{
			IsPasswordFormatInvalid = true;
		}
		else if (!EntityModel.ComparePasswords(Password, value))
		{
			ArePasswordsMismatch = true;
		}
		else
		{
			CreateModel();
		}
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
}
