using System.Text.RegularExpressions;

namespace application.MVVM.Model
{
	class AuthModel
	{
		public static string? Email { get; set; }
		public static string? Password { get; set; }

		public AuthModel() { }

		// TODO - ну наверное валидацию сюда нада?

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
