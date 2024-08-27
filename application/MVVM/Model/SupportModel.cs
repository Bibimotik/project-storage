namespace application.MVVM.Model;

public class SupportModel
{
	public string Email { get; set; } = string.Empty;
	public string Message { get; set; } = string.Empty;
	
	public List<byte[]> Screenshots { get; set; } = new();
	
	public static SupportModel Model { get; set; } = new();
}