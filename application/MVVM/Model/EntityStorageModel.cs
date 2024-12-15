namespace application.MVVM.Model;

public class EntityStorageModel
{
	public Guid Id { get; set; }
	public Guid Entity_ID { get; set; }
	public string Point { get; set; } = string.Empty;
	public string Country { get; set; } = string.Empty;
	public string City { get; set; } = string.Empty;
	public string Address { get; set; } = string.Empty;
	public string Index { get; set; } = string.Empty;

	public EntityStorageModel()
	{

	}

	public EntityStorageModel(Guid id, string point, string country, string city, string address, string index)
	{
		Id = id;
		Point = point;
		Country = country;
		City = city;
		Address = address;
		Index = index;
	}
}