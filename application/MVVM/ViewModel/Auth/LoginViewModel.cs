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

	// TODO - здесь скорее всего сделать валидацию самой почты, чтобы в этом компоненте выкидывать красный текст под инпутом что email не подходит
	partial void OnEmailChanged(string value)
	{
		if (!LoginModel.IsValidEmail(value))
		{
			// TODO - уведомление под инпутом что почта не валидна
			IsInvalidEmail = true;
			return;
		}
		LoginModel.Email = value;
		IsInvalidEmail = false;
	}
	partial void OnPasswordChanged(string value)
	{
		LoginModel.Password = value;
	}
}
