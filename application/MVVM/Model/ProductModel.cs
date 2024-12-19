namespace application.MVVM.Model;

public class ProductModel : EntityStorageModel
{
	public Guid Id { get; set; }
	public Guid Entity_Id { get; set; }
	public string Code { get; set; } = string.Empty;
	public string Title { get; set; } = string.Empty;
	public string Unit { get; set; } = string.Empty;
	public double Price { get; set; }
	public byte[] Image { get; set; } = [];
	public Guid Entity_Storage_ID { get; set; }
	public double Available_For_Shipment { get; set; }
	public string Party { get; set; } = string.Empty;
	public DateTime Implementation_Period { get; set; }
	public DateTime Expiration_Date { get; set; }

	public ProductModel() { }

	public ProductModel(
		Guid id,
		Guid entityId,
		string code,
		string title,
		string unit,
		double price,
		byte[] image,
		Guid entityStorageId,
		double availableForShipment,
		string party,
		DateTime implementationPeriod,
		DateTime expirationDate)
	{
		Id = id;
		Entity_Id = entityId;
		Code = code;
		Title = title;
		Unit = unit;
		Price = price;
		Image = image;
		Entity_Storage_ID = entityStorageId;
		Available_For_Shipment = availableForShipment;
		Party = party;
		Implementation_Period = implementationPeriod;
		Expiration_Date = expirationDate;
	}
}