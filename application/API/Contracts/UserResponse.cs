namespace application.API.Contracts;

public record UserResponse(
	Guid Id,
	string FirstName,
	string SecondName,
	string ThirdName,
	string Phone,
	string Email,
	string Password,
	string Logo,
	string Type);
