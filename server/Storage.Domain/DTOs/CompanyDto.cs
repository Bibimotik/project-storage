namespace Storage.Domain.DTOs;

public record CompanyDto(
	Guid Id,
	string Inn,
	string KPP,
	string FullName,
	string ShortName,
	string LegalAddress,
	string PostalAddress,
	string OGRN,
	string Director,
	string Email,
	string Password,
	string Logo,
	string type);

