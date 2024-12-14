using System;

namespace application.MVVM.Model
{
	public class ProductDataResult
	{
		public Guid Id { get; set; }
		public string Code { get; set; }
		public string Title { get; set; }
		public string Unit { get; set; }
		public double Price { get; set; }
		public byte[]? Image { get; set; }
		public double AvailableForShipment { get; set; }
		public string Party { get; set; }
		public DateTime ImplementationPeriod { get; set; }
		public DateTime ExpirationDate { get; set; }
	}
}
