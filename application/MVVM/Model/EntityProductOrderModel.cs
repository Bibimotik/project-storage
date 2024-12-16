namespace application.MVVM.Model;

public class EntityProductOrderModel
{
	public Guid ID { get; set; }
	public Guid Entity_Product_ID { get; set; }
	public Guid Order_ID { get; set; }
	public int Count { get; set; }

	public EntityProductOrderModel()
	{

	}

	public EntityProductOrderModel(Guid id, Guid entityProductId, Guid orderId, int count)
	{
		ID = id;
		Entity_Product_ID = entityProductId;
		Order_ID = orderId;
		Count = count;
	}
}