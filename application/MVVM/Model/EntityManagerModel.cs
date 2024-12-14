namespace application.MVVM.Model;

public class EntityManagerModel
{
	public Guid ID { get; set; }
	public Guid Entity_ID { get; set; }
	public Guid User_ID { get; set; }
	public string Access { get; set; } = string.Empty;

	public EntityManagerModel(Guid iD, Guid entity_ID, Guid user_ID, string access)
	{
		ID = iD;
		Entity_ID = entity_ID;
		User_ID = user_ID;
		Access = access;
	}
}
