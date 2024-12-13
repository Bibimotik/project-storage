using System.ComponentModel;

namespace application.Abstraction;

public class EntityAbstraction
{
	public enum EntityType
	{
		[Description(nameof(User))]
		User,
		[Description(nameof(Company))]
		Company,
		[Description(nameof(Support))]
		Support
	}

	public enum UserProperties
	{
		FirstName,
		SecondName,
		ThirdName,
		Phone,
		Email,
		Password,
		ConfirmPassword
	}

	public enum CompanyProperties
	{
		INN,
		KPP,
		FullName,
		ShortName,
		LegalAddress,
		PostalAddress,
		OGRN,
		Director,
		Email,
		Password,
		ConfirmPassword
	}

	public enum CompanyRegistrationStages
	{
		First,
		Second
	}
	
	public enum SupportProperties
	{
		Email,
		Message
	}
}
