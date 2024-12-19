namespace application.MVVM.Model;

public class ProductOrderModel : ProductModel
{
	public Guid ID { get; set; }
	public Guid Product_ID { get; set; }
	public Guid Order_ID { get; set; }
	public int Count { get; set; }
}
