namespace application.MVVM.Model;

public class EntityProductModel : EntityStorageModel
{
	public Guid ID { get; set; }
	public Guid Product_ID { get; set; }
	public Guid Entity_Storage_ID { get; set; }
	public double Available_For_Shipment { get; set; }
	public int In_Shipping_Area { get; set; }
	public int In_Reserve { get; set; }
	public string Party { get; set; } = string.Empty;
	public DateTime Implementation_Period { get; set; }
	public DateTime Expiration_Date { get; set; }

	public EntityProductModel()
	{

	}

	public EntityProductModel(
		Guid id,
		Guid productId,
		Guid entityStorageId,
		double availableForShipment,
		int inShippingArea,
		int inReserve,
		string party,
		DateTime implementationPeriod,
		DateTime expirationDate)
	{
		ID = id;
		Product_ID = productId;
		Entity_Storage_ID = entityStorageId;
		Available_For_Shipment = availableForShipment;
		In_Shipping_Area = inShippingArea;
		In_Reserve = inReserve;
		Party = party;
		Implementation_Period = implementationPeriod;
		Expiration_Date = expirationDate;
	}
}