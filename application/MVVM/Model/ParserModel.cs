namespace application.MVVM.Model;

public class ParserModel
{
	public string Kpp { get; set; }
	public string FullName { get; set; }
	public string ShortName { get; set; }
	public string Ogrn { get; set; }
	public string Director { get; set; }
	
	public static ParserModel Model { get; set; } = new();
}