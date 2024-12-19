namespace application.Abstraction.Interfaces;

public interface IStatisticsRepository
{
	public Task<IEnumerable<(DateTime Date, int Count)>> GetOrdersByDateAsync(Guid entityId, DateTime startDate,
		DateTime endDate);

	public Task<IEnumerable<(string Product, int TotalCount)>> GetProductSalesByCategoryAsync(Guid entityId,
		DateTime startDate, DateTime endDate);

	public Task<IEnumerable<(string Product, double Percentage)>> GetProductSalesPercentageAsync(Guid entityId,
		DateTime startDate, DateTime endDate);
}