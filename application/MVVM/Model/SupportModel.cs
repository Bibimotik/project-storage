using System.Net.Mime;

namespace application.MVVM.Model;

public class SupportModel
{
	public Guid Id { get; set; }
	public Guid Entity_ID { get; set; }
	public string Email { get; set; }
	public string Message { get; set; } = string.Empty;
	public byte[]? Image { get; set; }

	public SupportModel()
	{

	}
	
	public SupportModel(Guid id, Guid entityId, string email, string message, byte[] image)
	{
		Id = id;
		Entity_ID = entityId;
		Email = email;
		Message = message;
		Image = image;
	}
}