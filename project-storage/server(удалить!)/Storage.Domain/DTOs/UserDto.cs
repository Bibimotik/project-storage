using Storage.Domain.Abstractions;

namespace Storage.Domain.DTOs;

public record UserDto(
	Guid Id,
	string FirstName,
	string SecondName,
	string ThirdName,
	string Phone,
	string Email,
	string Password,
	string Logo,
	string type);

