using System.ComponentModel.DataAnnotations;

namespace Storage.API.Contracts.Users;

public class CreateUserRegistrationRequest
{
	[Required]
	public string FirstName { get; set; }

	[Required]
	public string SecondName { get; set; }

	[Required]
	public string ThirdName { get; set; }

	[Required]
	public string Phone { get; set; }

	[Required]
	[EmailAddress]
	public string Email { get; set; }

	[Required]
	public string Password { get; set; }

	[Required]
	[Compare(nameof(Password), ErrorMessage = "Passwords do not match.")]
	public string PasswordConfirmation { get; set; }

	[Required]
	public string Type { get; set; }

	public byte[] Logo { get; set; }
}
