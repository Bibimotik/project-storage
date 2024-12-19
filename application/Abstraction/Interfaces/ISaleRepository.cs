using application.MVVM.Model;

namespace application.Abstraction.Interfaces;

public interface ISaleRepository
{
	Task<OrderModel> GetOrderByIdAsync(Guid saleId);
	public Task<IEnumerable<OrderModel>> GetOrdersByEntityIdAsync(Guid entityId, string searchQuery = "",
		string orderBy = "");
	public Task<IEnumerable<OrderModel>> GetOrdersByOrderIdAsync(Guid orderId);
	public Task<bool> MarkSaleAsDeletedAsync(Guid saleId);
}