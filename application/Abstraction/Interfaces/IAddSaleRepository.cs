using application.MVVM.Model;

namespace application.Abstraction.Interfaces;

public interface IAddSaleRepository
{
	public Task<Guid> InsertOrder(OrderModel orderModel);
	public Task<Guid> GetStorage(string storage);
}