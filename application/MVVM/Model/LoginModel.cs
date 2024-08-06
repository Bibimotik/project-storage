using System.Text.RegularExpressions;

namespace application.MVVM.Model
{
	class LoginModel
	{
		public static string? Email { get; set; }
		public static string? Password { get; set; }
		public static string? ConfirmPassword { get; set; }

		public LoginModel() { }

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
