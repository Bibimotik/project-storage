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
		RegistrationModel.Model ??= new RegistrationModel();

		RegistrationModel model = RegistrationModel.Model;
		model.Director = Director;
		model.Email = Email;
		model.Password = Password;
		model.ConfirmPassword = ConfirmPassword;

		Debug.WriteLine($"director: {model.Director}\n" +
			$"email: {model.Email}\n" +
			$"password: {model.Password}\n" +
			$"confirmPassword: {model.ConfirmPassword}");
		RegistrationModel.SLAVA = "slava";
		Debug.WriteLine("slava: " + RegistrationModel.SLAVA);
	}
}
