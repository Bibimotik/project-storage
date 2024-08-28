namespace application.Abstraction;

public class EntityAbstraction
{
	public enum EntityType
	{
		User,
		Company,
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
