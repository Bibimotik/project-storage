namespace application.MVVM.Model;

public class DeleteModel
{
	public string Type { get; set; }
	public Guid Type_Id { get; set; }
	
	public DeleteModel(string type, Guid typeId)
	{
		Type = type;
		Type_Id = typeId;
	}
}