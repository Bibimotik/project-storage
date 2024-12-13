namespace application.API.Contracts;

public class ErrorResponse
{
	public string Title { get; set; }
	public int Status { get; set; }
	public string Detail { get; set; }
}