namespace application.MVVM.Model;

public class StaffModel
{
	public Guid Id { get; set; }
	public Guid EntityId { get; set; }
	public Guid UserId { get; set; }
	public string Access { get; set; } = string.Empty;

	public StaffModel()
	{

	}

	public StaffModel(Guid id, Guid entityId, Guid userId, string access)
	{
		Id = id;
		EntityId = entityId;
		UserId = userId;
		Access = access;
	}
}