namespace application.MVVM.Model;

public class EntityStorageModel
{
	public Guid Id { get; set; }
	public string Point { get; set; } = null!;
	public string Country { get; set; } = null!;
	public string City { get; set; } = null!;
	public string Address { get; set; } = null!;
	public string Index { get; set; } = null!;

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