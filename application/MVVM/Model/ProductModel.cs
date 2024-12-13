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

	public ProductModel(Guid id, Guid entityId, string code, string title, string unit, double price)
	{
		Id = id;
		EntityId = entityId;
		Code = code;
		Title = title;
		Unit = unit;
		Price = price;
	}
	
	public ProductModel(Guid id, Guid entityId, string code, string title, string unit, double price, byte[] image)
	{
		Id = id;
		EntityId = entityId;
		Code = code;
		Title = title;
		Unit = unit;
		Price = price;
		Image = image;
	}
}