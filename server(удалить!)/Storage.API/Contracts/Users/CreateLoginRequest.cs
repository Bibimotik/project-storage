using System.ComponentModel.DataAnnotations;

namespace Storage.API.Contracts.Users;

public class CreateLoginRequest
{
	[Required]
	[EmailAddress]
	public string Email { get; set; }

	[Required]
	public string Password { get; set; }
}