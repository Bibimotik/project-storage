using System.ComponentModel.DataAnnotations;

namespace Storage.API.Contracts.Users;

public class CreateUserRegistrationRequest
{
	public string FirstName { get; set; }

	public string SecondName { get; set; }

	public string ThirdName { get; set; }

	public string Phone { get; set; }

	[EmailAddress]
	public string Email { get; set; }

	public string Password { get; set; }

	public string ConfirmPassword { get; set; }

	public string EntityType { get; set; }

	public byte[] Logo { get; set; }
}
