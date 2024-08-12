using System.Text.RegularExpressions;

namespace application.MVVM.Model;

public class EntityModel
{
	public Guid Id { get; set; }
	public string Email { get; set; } = string.Empty;
	public string Password { get; set; } = string.Empty;
	public string ConfirmPassword { get; set; } = string.Empty;
	public string FirstName { get; set; } = string.Empty;
	public string SecondName { get; set; } = string.Empty;
	public string ThirdName { get; set; } = string.Empty;
	public string Phone { get; set; } = string.Empty;
	public string INN { get; set; } = string.Empty;
	public string KPP { get; set; } = string.Empty;
	public string FullName { get; set; } = string.Empty;
	public string ShortName { get; set; } = string.Empty;
	public string LegalAddress { get; set; } = string.Empty;
	public string PostalAddress { get; set; } = string.Empty;
	public string OGRN { get; set; } = string.Empty;
	public string Director { get; set; } = string.Empty;
	public string Code { get; set; } = string.Empty;
	public static EntityModel Model { get; set; } = new();

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
