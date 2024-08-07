namespace application.MVVM.Model
{
	class RegistrationModel : LoginModel
	{
		public string INN { get; set; } = string.Empty;
		public string KPP { get; set; } = string.Empty;
		public string FullName { get; set; } = string.Empty;
		public string ShortName { get; set; } = string.Empty;
		public string LegalAddress { get; set; } = string.Empty;
		public string PostalAddress { get; set; } = string.Empty;
		public string OGRN { get; set; } = string.Empty;
		public string Director { get; set; } = string.Empty;
		public string FirstName { get; set; } = string.Empty;
		public string SecondName { get; set; } = string.Empty;
		public string ThirdName { get; set; } = string.Empty;
		public string Phone { get; set; } = string.Empty;
		public static new RegistrationModel Model { get; set; } = new();

		public RegistrationModel() { }

		private RegistrationModel(string email, string password, string confirmPassword) : base(email, password, confirmPassword) { }

		public RegistrationModel(string director, string email, string password, string confirmPassword) : base(email, password, confirmPassword)
		{
			Director = director;
		}

		public static RegistrationModel CreateCompany(string inn, string kpp, string fullName, string shortName, string legalAddress, string postalAddress, string ogrn)
		{
			return new RegistrationModel
			{
				INN = inn,
				KPP = kpp,
				FullName = fullName,
				ShortName = shortName,
				LegalAddress = legalAddress,
				PostalAddress = postalAddress,
				OGRN = ogrn
			};
		}

		public static RegistrationModel CreateUser(string firstName, string secondName, string thirdName, string phone, string email, string password, string confirmPassword)
		{
			return new RegistrationModel(email, password, confirmPassword)
			{
				FirstName = firstName,
				SecondName = secondName,
				ThirdName = thirdName,
				Phone = phone
			};
		}
	}
}
