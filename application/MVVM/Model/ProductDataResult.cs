namespace application.MVVM.Model;

public class ProductDataResult
{
	public Guid Id { get; set; }
	public string Code { get; set; } = string.Empty;
	public string Title { get; set; } = string.Empty;
	public string Unit { get; set; } = string.Empty;
	public double Price { get; set; }
	public byte[]? Image { get; set; }
	public double AvailableForShipment { get; set; }
	public string Party { get; set; } = string.Empty;
	public DateTime ImplementationPeriod { get; set; }
	public DateTime ExpirationDate { get; set; }

	public ProductDataResult()
	{

	}
}