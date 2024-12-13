using System.Net.Mime;

namespace application.MVVM.Model;

public class SupportModel
{
	public Guid ID { get; set; }
	public Guid Entity_ID { get; set; }
	public string Message { get; set; } = null!;
	public byte[] Image { get; set; }

	public SupportModel(Guid id, Guid entityId, string message)
	{
		ID = id;
		Entity_ID = entityId;
		Message = message;
	}
	
	public SupportModel(Guid id, Guid entityId, string message, byte[] image)
	{
		ID = id;
		Entity_ID = entityId;
		Message = message;
		Image = image;
	}
}