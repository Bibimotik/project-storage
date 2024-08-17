using System.Text.RegularExpressions;

using application.Utilities;

using static application.Abstraction.EntityAbstraction;

namespace application.MVVM.Model;

public class EntityModel
{
	public Guid Id { get; set; }
	
	[RequiredForValidation]
	public string? FirstName { get; set; } = null;
	[RequiredForValidation]
	public string? SecondName { get; set; } = null;
	[RequiredForValidation]
	public string? ThirdName { get; set; } = null;
	[RequiredForValidation]
	public string? Phone { get; set; } = null;
	
	[RequiredForValidation]
	public string Email { get; set; } = string.Empty;
	[RequiredForValidation]
	public string Password { get; set; } = string.Empty;
	[RequiredForValidation]
	public string ConfirmPassword { get; set; } = string.Empty;

	[RequiredForValidation]
	public string INN { get; set; } = string.Empty;
	[RequiredForValidation]
	public string KPP { get; set; } = string.Empty;

	[RequiredForValidation]
	public string FullName { get; set; } = string.Empty;
	[RequiredForValidation]
	public string ShortName { get; set; } = string.Empty;

	[RequiredForValidation]
	public string LegalAddress { get; set; } = string.Empty;
	[RequiredForValidation]
	public string PostalAddress { get; set; } = string.Empty;

	[RequiredForValidation]
	public string OGRN { get; set; } = string.Empty;
	[RequiredForValidation]
	public string Director { get; set; } = string.Empty;

	public string Code { get; set; } = string.Empty;
	public string InputCode { get; set; } = string.Empty;

	public EntityType EntityType { get; set; }
	public static EntityModel Model { get; set; } = new();


	public static bool IsValidEmail(string email)
	{
		if (string.IsNullOrWhiteSpace(email))
			return false;
		try
		{
			var regex = new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
			return regex.IsMatch(email);
		}
		catch (RegexMatchTimeoutException)
		{
			return false;
		}
	}
	
	public static bool ComparePasswords(string password, string confirmPassword) => password == confirmPassword;
}
