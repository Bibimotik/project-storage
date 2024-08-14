using System.Diagnostics;

using application.MVVM.Model;

using CommunityToolkit.Mvvm.ComponentModel;

namespace application.MVVM.ViewModel.Auth;

partial class LoginViewModel : ObservableObject
{
	[ObservableProperty]
	private string email = string.Empty;
	[ObservableProperty]
	private string password = string.Empty;
	[ObservableProperty]
	private bool isInvalidEmail = false;

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
