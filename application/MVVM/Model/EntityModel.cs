using System.Text.RegularExpressions;

using application.Utilities;

using static application.Abstraction.EntityAbstraction;

namespace application.MVVM.Model;

// TODO - возможно можно убрать атрибут RequiredForValidation и его класса, но может пригодитмся для какой то странной логику в будущем
public class EntityModel
{
	public Guid Id { get; set; }

	[RequiredForValidation]
	[RequiredForUser]
	public string FirstName { get; set; } = string.Empty;

	[RequiredForValidation]
	[RequiredForUser]
	public string SecondName { get; set; } = string.Empty;

	[RequiredForValidation]
	[RequiredForUser]
	public string ThirdName { get; set; } = string.Empty;

	[RequiredForValidation]
	[RequiredForUser]
	public string Phone { get; set; } = string.Empty;

	[RequiredForValidation]
	[RequiredForCompany1]
	public string INN { get; set; } = string.Empty;

	[RequiredForValidation]
	[RequiredForCompany1]
	public string KPP { get; set; } = string.Empty;

	[RequiredForValidation]
	[RequiredForCompany1]
	public string FullName { get; set; } = string.Empty;

	[RequiredForValidation]
	[RequiredForCompany1]
	public string ShortName { get; set; } = string.Empty;

	[RequiredForValidation]
	[RequiredForCompany1]
	public string LegalAddress { get; set; } = string.Empty;

	[RequiredForValidation]
	[RequiredForCompany1]
	public string PostalAddress { get; set; } = string.Empty;

	[RequiredForValidation]
	[RequiredForCompany1]
	public string OGRN { get; set; } = string.Empty;

	[RequiredForValidation]
	[RequiredForCompany2]
	[RequiredForSupport]
	public string Director { get; set; } = string.Empty;

	[RequiredForValidation]
	[RequiredForUser]
	[RequiredForCompany2]
	public string Email { get; set; } = string.Empty;

	[RequiredForValidation]
	[RequiredForUser]
	[RequiredForCompany2]
	public string Password { get; set; } = string.Empty;

	[RequiredForValidation]
	[RequiredForUser]
	[RequiredForCompany2]
	public string ConfirmPassword { get; set; } = string.Empty;
	[RequiredForSupport]
	public string Message { get; set; } = string.Empty;
	public byte[] Image { get; set; } = Array.Empty<byte>();

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
}
