using application.MVVM.Model;

namespace application.Abstraction.Interfaces;

public interface ISaleRepository
{
	public Task<IEnumerable<OrderModel>> GetOrdersByEntityIdAsync(Guid entityId, string searchQuery = "",
		string orderBy = "");
}