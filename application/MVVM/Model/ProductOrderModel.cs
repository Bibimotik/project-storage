namespace application.MVVM.Model;

public class ProductOrderModel
{
	public Guid ID { get; set; }
	public Guid Product_ID { get; set; }
	public Guid Order_ID { get; set; }
	public int Count { get; set; }
}
