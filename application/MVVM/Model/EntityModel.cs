using System.Text.RegularExpressions;

using application.Utilities;

using static application.Abstraction.EntityAbstraction;

namespace application.MVVM.Model;

// TODO - возможно можно убрать атрибут RequiredForValidation и его класса, но может пригодитмся для какой то странной логику в будущем
// TODO - но так оно все равно перекрывается двумя другими атрибутами - для User и Company
public class EntityModel
{
	public Guid Id { get; set; }

	[RequiredForValidation]
	[RequiredForUser]
	public string? FirstName { get; set; } = null;
	[RequiredForValidation]
	[RequiredForUser]
	public string? SecondName { get; set; } = null;
	[RequiredForValidation]
	[RequiredForUser]
	public string? ThirdName { get; set; } = null;
	[RequiredForValidation]
	[RequiredForUser]
	public string? Phone { get; set; } = null;

	[RequiredForValidation]
	[RequiredForCompany1]
	public string? INN { get; set; } = null;
	[RequiredForValidation]
	[RequiredForCompany1]
	public string? KPP { get; set; } = null;

	[RequiredForValidation]
	[RequiredForCompany1]
	public string? FullName { get; set; } = null;
	[RequiredForValidation]
	[RequiredForCompany1]
	public string? ShortName { get; set; } = null;

	[RequiredForValidation]
	[RequiredForCompany1]
	public string? LegalAddress { get; set; } = null;
	[RequiredForValidation]
	[RequiredForCompany1]
	public string? PostalAddress { get; set; } = null;

	[RequiredForValidation]
	[RequiredForCompany1]
	public string? OGRN { get; set; } = null;
	[RequiredForValidation]
	[RequiredForCompany2]
	public string? Director { get; set; } = null;

	[RequiredForValidation]
	[RequiredForUser]
	[RequiredForCompany2]
	public string? Email { get; set; } = null;
	[RequiredForValidation]
	[RequiredForUser]
	[RequiredForCompany2]
	public string? Password { get; set; } = null;
	[RequiredForValidation]
	[RequiredForUser]
	[RequiredForCompany2]
	public string? ConfirmPassword { get; set; } = null;

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
