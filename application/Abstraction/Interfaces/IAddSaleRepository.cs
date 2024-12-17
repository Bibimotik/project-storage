using application.MVVM.Model;

namespace application.Abstraction.Interfaces;

public interface IAddSaleRepository
{
	public Task<Guid> InsertOrder(OrderModel orderModel);
	public Task<Guid> GetStorage(Guid entityId, string storage);
	public Task<IEnumerable<ProductDataResult>> GetProductsDataAsync(Guid storageId);
	public Task InsertProductOrder(ProductOrderModel productOrderModel);
	public Task<CompanyDataResult> GetCompanyDataByEntityId(Guid entityId);
}