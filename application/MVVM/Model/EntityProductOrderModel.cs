namespace application.MVVM.Model;

public class EntityProductOrderModel
{
	public Guid ID { get; set; }
	public Guid Product_ID { get; set; }
	public Guid Order_ID { get; set; }
	public int Count { get; set; }

	public EntityProductOrderModel()
	{

	}

	public EntityProductOrderModel(Guid id, Guid entityProductId, Guid orderId, int count)
	{
		ID = id;
		Product_ID = entityProductId;
		Order_ID = orderId;
		Count = count;
	}
}