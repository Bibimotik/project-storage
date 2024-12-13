using System.Data;

using Storage.Domain.Abstractions;

namespace Storage.Domain.Models;

public class EntityModel
{
	public Guid Id { get; set; }

	public string FirstName { get; set; } = string.Empty;

	public string SecondName { get; set; } = string.Empty;

	public string ThirdName { get; set; } = string.Empty;

	public string Phone { get; set; } = string.Empty;

	public string INN { get; set; } = string.Empty;

	public string KPP { get; set; } = string.Empty;

	public string FullName { get; set; } = string.Empty;

	public string ShortName { get; set; } = string.Empty;

	public string LegalAddress { get; set; } = string.Empty;

	public string PostalAddress { get; set; } = string.Empty;

	public string OGRN { get; set; } = string.Empty;

	public string Director { get; set; } = string.Empty;

	public string Email { get; set; } = string.Empty;

	public string Password { get; set; } = string.Empty;

	public byte[]? Logo { get; set; } = [];

	public string Message { get; set; } = string.Empty;
	public byte[]? Images { get; set; } = [];

	public EntityType Type { get; set; }

	public EntityModel() { }

	public EntityModel(Guid id,
					string firstName,
					string secondName,
					string thirdName,
					string phone,
					string email,
					string password,
					EntityType type,
					byte[] logo)
	{
		Id = id;
		FirstName = firstName;
		SecondName = secondName;
		ThirdName = thirdName;
		Phone = phone;
		Email = email;
		Password = password;
		Type = type;
		Logo = logo;
	}
}