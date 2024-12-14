using System.CodeDom;

namespace application.MVVM.Model;

public class ProductModel
{
	public Guid Id { get; set; }
	public Guid EntityId { get; set; }
	public string Code { get; set; } = null!;
	public string Title { get; set; } = null!;
	public string Unit { get; set; } = null!;
	public double Price { get; set; }
	public byte[] Image { get; set; }
	public Guid Entity_Storage_ID { get; set; }
	public double Available_For_Shipment { get; set; }
	public string Party { get; set; } = null!;
	public DateTime Implementation_Period { get; set; }
	public DateTime Expiration_Date { get; set; }
}