namespace application.MVVM.Model;

public class ParserModel
{
	public string Kpp { get; set; } = string.Empty;
	public string FullName { get; set; } = string.Empty;
	public string ShortName { get; set; } = string.Empty;
	public string Ogrn { get; set; } = string.Empty;
	public string Director { get; set; } = string.Empty;

	public static ParserModel Model { get; set; } = new();

	public ParserModel()
	{

	}
}