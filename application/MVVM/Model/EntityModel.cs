using System.Text.RegularExpressions;

using application.Utilities;

using static application.Abstraction.EntityAbstraction;

namespace application.MVVM.Model;

// TODO - возможно можно убрать атрибут RequiredForValidation и его класса, но может пригодитмся для какой то странной логику в будущем
public class EntityModel : ProductOrderModel
{
	[RequiredForUserTable]
	[RequiredForCompanyTable]
	public Guid Id { get; set; }

	[RequiredForUserTable]
	[RequiredForValidation]
	[RequiredForUser]
	public string FirstName { get; set; } = string.Empty;

	[RequiredForUserTable]
	[RequiredForValidation]
	[RequiredForUser]
	public string SecondName { get; set; } = string.Empty;

	[RequiredForUserTable]
	[RequiredForValidation]
	[RequiredForUser]
	public string ThirdName { get; set; } = string.Empty;

	[RequiredForUserTable]
	[RequiredForValidation]
	[RequiredForUser]
	public string Phone { get; set; } = string.Empty;

	[RequiredForCompanyTable]
	[RequiredForValidation]
	[RequiredForCompany1]
	public string INN { get; set; } = string.Empty;

	[RequiredForCompanyTable]
	[RequiredForValidation]
	[RequiredForCompany1]
	public string KPP { get; set; } = string.Empty;

	[RequiredForCompanyTable]
	[RequiredForValidation]
	[RequiredForCompany1]
	public string FullName { get; set; } = string.Empty;

	[RequiredForCompanyTable]
	[RequiredForValidation]
	[RequiredForCompany1]
	public string ShortName { get; set; } = string.Empty;

	[RequiredForCompanyTable]
	[RequiredForValidation]
	[RequiredForCompany1]
	public string LegalAddress { get; set; } = string.Empty;

	[RequiredForCompanyTable]
	[RequiredForValidation]
	[RequiredForCompany1]
	public string PostalAddress { get; set; } = string.Empty;

	[RequiredForCompanyTable]
	[RequiredForValidation]
	[RequiredForCompany1]
	public string OGRN { get; set; } = string.Empty;

	[RequiredForCompanyTable]
	[RequiredForValidation]
	[RequiredForCompany2]
	public string Director { get; set; } = string.Empty;

	[RequiredForCompanyTable]
	[RequiredForUserTable]
	[RequiredForValidation]
	[RequiredForUser]
	[RequiredForCompany2]
	[RequiredForSupport]
	[RequiredForLogin]
	public string Email { get; set; } = string.Empty;

	[RequiredForCompanyTable]
	[RequiredForUserTable]
	[RequiredForValidation]
	[RequiredForUser]
	[RequiredForCompany2]
	[RequiredForLogin]
	public string Password { get; set; } = string.Empty;

	[RequiredForValidation]
	[RequiredForUser]
	[RequiredForCompany2]
	public string ConfirmPassword { get; set; } = string.Empty;

	// TODO - Что это вообще за поле такое, я не помню
	[RequiredForSupport]
	public string Message { get; set; } = string.Empty;
	[RequiredForUserTable]
	[RequiredForCompanyTable]
	public byte[]? Logo { get; set; } = [];
	public byte[]? Images { get; set; } = [];

	public string Code { get; set; } = string.Empty;
	public string InputCode { get; set; } = string.Empty;

	public Guid EntityId { get; set; }
	public UserRole Role { get; set; } = UserRole.NoRole;

	public EntityType EntityType { get; set; }
	public static EntityModel Model { get; set; } = new();
	public static EntityModel OurUserModel { get; set; } = new();

	public object this[string propertyName]
	{
		get => GetType().GetProperty(propertyName)?.GetValue(this);
		set => GetType().GetProperty(propertyName)?.SetValue(this, value);
	}

	public EntityModel()
	{

	}

	public EntityModel(
		Guid id,
		string firstName,
		string secondName,
		string thirdName,
		string phone,
		string email,
		string password,
		EntityType entityType,
		byte[] logo)
	{
		Id = id;
		FirstName = firstName;
		SecondName = secondName;
		ThirdName = thirdName;
		Phone = phone;
		Email = email;
		Password = password;
		EntityType = entityType;
		Logo = logo;
	}

	public EntityModel(
		Guid id,
		string inn,
		string kpp,
		string fullname,
		string shortname,
		string legalAddress,
		string postalAddresss,
		string director,
		string email,
		string password,
		EntityType entityType,
		byte[] logo)
	{
		Id = id;
		Email = email;
		Password = password;
		EntityType = entityType;
		Logo = logo;
	}

	//public static bool IsValidEmail(string email)
	//{
	//	if (string.IsNullOrWhiteSpace(email))
	//		return false;
	//	try
	//	{
	//		var regex = new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
	//		return regex.IsMatch(email);
	//	}
	//	catch (RegexMatchTimeoutException)
	//	{
	//		return false;
	//	}
	//}

	public static bool IsValidEmail(string email)
	{
		if (string.IsNullOrWhiteSpace(email))
			return false;
		try
		{
			var address = new System.Net.Mail.MailAddress(email);
			return address.Address == email;
		}
		catch
		{
			return false;
		}
	}

	private void Clear()
	{
		Id = Guid.Empty;
		FirstName = string.Empty;
		SecondName = string.Empty;
		ThirdName = string.Empty;
		Phone = string.Empty;
		INN = string.Empty;
		KPP = string.Empty;
		FullName = string.Empty;
		ShortName = string.Empty;
		LegalAddress = string.Empty;
		PostalAddress = string.Empty;
		OGRN = string.Empty;
		Director = string.Empty;
		Email = string.Empty;
		Password = string.Empty;
		ConfirmPassword = string.Empty;
		Message = string.Empty;
		Images = [];
		Code = string.Empty;
		InputCode = string.Empty;
		EntityType = default;
	}

	public static void Reset()
	{
		Model.Clear();
	}

	public static void ResetOurUser()
	{
		OurUserModel.Clear();
	}
}