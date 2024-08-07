using System.Text.RegularExpressions;

namespace application.MVVM.Model
{
	class LoginModel
	{
		public string Email { get; set; } = string.Empty;
		public string Password { get; set; } = string.Empty;
		public string ConfirmPassword { get; set; } = string.Empty;
		public static LoginModel Model { get; set; } = new();

		public LoginModel() { }

		public LoginModel(string email, string password)
		{
			Email = email;
			Password = password;
		}

		public LoginModel(string email, string password, string confirmPassword)
		{
			Email = email;
			Password = password;
			ConfirmPassword = confirmPassword;
		}

		// TODO - ну наверное валидацию сюда нада?

		// TODO - virtual же не надо наверное
		public static bool IsValidEmail(string email)
		{
			if (string.IsNullOrWhiteSpace(email))
				return false;
			try
			{
				var regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
				return regex.IsMatch(email);
			}
			catch (RegexMatchTimeoutException)
			{
				return false;
			}
		}
	}
}
