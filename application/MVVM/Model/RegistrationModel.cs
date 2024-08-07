namespace application.MVVM.Model
{
	class RegistrationModel : LoginModel
	{
		public static string SLAVA { get; set; } = string.Empty;

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
	}
}
