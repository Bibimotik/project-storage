using System.Diagnostics;

using application.MVVM.Model;

using CommunityToolkit.Mvvm.ComponentModel;

using static application.Abstraction.EntityAbstraction;

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
		if (!EntityModel.IsValidEmail(value))
		{
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
		EntityModel.Model ??= new EntityModel();

		EntityModel model = EntityModel.Model;
		model.EntityType = EntityType.Company;
		model.Director = Director;
		model.Email = Email;
		model.Password = Password;
		model.ConfirmPassword = ConfirmPassword;

		Debug.WriteLine($"director: {model.Director}\n" +
			$"email: {model.Email}\n" +
			$"password: {model.Password}\n" +
			$"confirmPassword: {model.ConfirmPassword}");
	}
}
