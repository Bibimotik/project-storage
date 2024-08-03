using System.Diagnostics;
using System.Text.RegularExpressions;

using application.MVVM.Model;

using CommunityToolkit.Mvvm.ComponentModel;

namespace application.MVVM.ViewModel.Auth;

partial class LoginViewModel : ObservableObject
{
	[ObservableProperty]
	private string email = string.Empty;
	[ObservableProperty]
	private string password = string.Empty;

	// TODO - здесь скорее всего сделать валидацию самой почты, чтобы в этом компоненте выкидывать красный текст под инпутом что email не подходит
	partial void OnEmailChanged(string value)
	{
		if (!IsValidEmail(value))
		{
			// TODO - уведомление под инпутом что почта не валидна
			Debug.WriteLine("Invalid Email...");
			return;
		}
		AuthModel.Email = value;
	}
	partial void OnPasswordChanged(string value)
	{
		AuthModel.Password = value;
	}

	private bool IsValidEmail(string email)
	{
		if (string.IsNullOrWhiteSpace(email))
			return false;


		try
		{
			// Используем регулярное выражение для проверки формата email
			var regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
			return regex.IsMatch(email);
		}
		catch (RegexMatchTimeoutException)
		{
			return false;
		}
	}
}
