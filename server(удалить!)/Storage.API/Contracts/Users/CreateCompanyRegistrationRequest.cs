using System.ComponentModel.DataAnnotations;

namespace Storage.API.Contracts.Users;

public class CreateCompanyRegistrationRequest
{
	[Required]
	public string INN { get; set; }

	[Required]
	public string KPP { get; set; }

	[Required]
	public string OGRN { get; set; }

	[Required]
	public string FullName { get; set; }

	[Required]
	public string ShortName { get; set; }

	[Required]
	[EmailAddress]
	public string Email { get; set; }

	[Required]
	public string Password { get; set; }

	[Required]
	[Compare(nameof(Password), ErrorMessage = "Passwords do not match.")]
	public string PasswordConfirmation { get; set; }

	[Required]
	public string LegalAddress { get; set; }

	[Required]
	public string PostalAddress { get; set; }

	[Required]
	public string Director { get; set; }

	[Required]
	public string EntityType { get; set; }

	public byte[] Logo { get; set; }
}
