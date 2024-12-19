namespace application.MVVM.Model;

public class RoleDataResult
{
	public Guid Id { get; set; }
	public string First { get; set; } = string.Empty;
	public string Second { get; set; } = string.Empty;
	public string Third { get; set; } = string.Empty;
	public byte[]? Logo { get; set; }
	public string Access { get; set; } = string.Empty;

	public RoleDataResult()
	{

	}
}
