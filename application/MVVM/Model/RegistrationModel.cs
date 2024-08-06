namespace application.MVVM.Model
{
	class RegistrationModel : LoginModel
	{
		public static string INN { get; set; } = string.Empty;
		public static string KPP { get; set; } = string.Empty;
		public static string FullName { get; set; } = string.Empty;
		public static string ShortName { get; set; } = string.Empty;
		public static string LegalAddress { get; set; } = string.Empty;
		public static string PostalAddress { get; set; } = string.Empty;
		public static string OGRN { get; set; } = string.Empty;
		public static string Director { get; set; } = string.Empty;
	}
}
