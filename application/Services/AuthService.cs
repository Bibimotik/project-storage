using System.Diagnostics;

using application.Abstraction;
using application.Properties;

namespace application.Services;

public class AuthService : IAuthService
{
	public void SaveAuthData(string authEmail, string authPassword)
	{
		Settings.Default.AuthEmail = authEmail;
		Settings.Default.AuthPassword = authPassword;
		Settings.Default.Save();
	}

	public (string authEmail, string authPassword) LoadAuthData()
	{
		var authEmail = Settings.Default.AuthEmail;
		var authPassword = Settings.Default.AuthPassword;

		Debug.WriteLine("!!!!!!!! " + authEmail + " " + authPassword);

		return (authEmail, authPassword);
	}

	public bool IsUserAuthenticated()
	{
		return !string.IsNullOrEmpty(Settings.Default.AuthEmail);
	}

	public void ClearAuthData()
	{
		Settings.Default.AuthEmail = string.Empty;
		Settings.Default.AuthPassword = string.Empty;
		Settings.Default.Save();
	}
}

