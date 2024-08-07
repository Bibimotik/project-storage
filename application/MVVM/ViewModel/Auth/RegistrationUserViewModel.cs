using System.Diagnostics;

using application.MVVM.Model;

using CommunityToolkit.Mvvm.ComponentModel;

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

	partial void OnFirstNameChanged(string value) => CreateModel();
	partial void OnSecondNameChanged(string value) => CreateModel();
	partial void OnThirdNameChanged(string value) => CreateModel();
	partial void OnPhoneChanged(string value) => CreateModel();
	partial void OnEmailChanged(string value)
	{
		if (!RegistrationModel.IsValidEmail(value))
		{
			// TODO - уведомление под инпутом что почта не валидна
			IsInvalidEmail = true;
			return;
		}
		IsInvalidEmail = false;
		CreateModel();
	}
	partial void OnPasswordChanged(string value) => CreateModel();
	partial void OnConfirmPasswordChanged(string value) => CreateModel();

	private void CreateModel()
	{
		RegistrationModel.Model = RegistrationModel.CreateUser(FirstName, SecondName, ThirdName, Phone, Email, Password, ConfirmPassword);

		//Debug.WriteLine($"director1: {RegistrationModel.Model.Director}\n" +
		//	$"email: {RegistrationModel.Model.Email}\n" +
		//	$"password: {RegistrationModel.Model.Password}\n" +
		//	$"confirmPassword: {RegistrationModel.Model.ConfirmPassword}");
	}
}
