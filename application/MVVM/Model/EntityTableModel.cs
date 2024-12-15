namespace application.MVVM.Model;

public class EntityTableModel
{
	public Guid Id { get; set; }
	public string Type { get; set; } = string.Empty;
	public Guid Type_ID { get; set; }

	public EntityTableModel()
	{

	}

	public EntityTableModel(Guid id, string type, Guid type_ID)
	{
		Id = id;
		Type = type;
		Type_ID = type_ID;
	}
}
