namespace application.MVVM.Model;

public class StorageDataResult
{
	public Guid Id { get; set; }
	public string Point { get; set; } = string.Empty;
	public string Country { get; set; } = string.Empty;
	public string City { get; set; } = string.Empty;
	public string Address { get; set; } = string.Empty;
	public string Index { get; set; } = string.Empty;

	public StorageDataResult()
	{

	}
}