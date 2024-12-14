namespace application.MVVM.Model;

public class EntityManagerModel
{
	public Guid Id { get; set; }
	public Guid User_ID { get; set; }
	public string Access { get; set; } = string.Empty;

	public EntityManagerModel(Guid id, Guid user_ID, string access)
	{
		Id = id;
		User_ID = user_ID;
		Access = access;
	}
}
