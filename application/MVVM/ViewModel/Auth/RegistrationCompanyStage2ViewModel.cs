using System.Diagnostics;

using application.MVVM.Model;

using CommunityToolkit.Mvvm.ComponentModel;

namespace application.MVVM.ViewModel.Auth;

partial class RegistrationCompanyStage2ViewModel : ObservableObject
{
	[ObservableProperty]
	private string director = string.Empty;
	[ObservableProperty]
	private string email = string.Empty;
	[ObservableProperty]
	private string password = string.Empty;
	[ObservableProperty]
	private string confirmPassword = string.Empty;
	[ObservableProperty]
	private bool isInvalidEmail = false;

	partial void OnDirectorChanged(string value) => CreateModel();
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
		RegistrationModel.Model = new RegistrationModel(Director, Email, Password, ConfirmPassword);

		//Debug.WriteLine($"director: {RegistrationModel.Model.Director}\n" +
		//	$"email: {RegistrationModel.Model.Email}\n" +
		//	$"password: {RegistrationModel.Model.Password}\n" +
		//	$"confirmPassword: {RegistrationModel.Model.ConfirmPassword}");
	}
}
