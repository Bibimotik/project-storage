using application.Abstraction;
using application.MVVM.Model;

namespace application.Abstraction.Interfaces;
public interface ITablesRepository
{
	Task DeleteData<T>(EntityAbstraction.TableNames tableName, Guid id);
	Task DeleteEntity(Guid id);

	public Task UpdateUser(EntityModel updateValues);
	Task<IList<T>> GetData<T>(EntityAbstraction.TableNames tableName);
	Task UpdateCompany(EntityModel updateValues);
	Task UpdateData(EntityAbstraction.TableNames tableName, EntityProductOrderModel updateValues);
	Task UpdateEntity(EntityTableModel updateValues);
	Task UpdateEntityStorage(EntityStorageModel updateValues);
	Task UpdateOrder(OrderModel updateValues);
	Task UpdateProduct(ProductModel updateValues);
	Task UpdateSupport(SupportModel updateValues);
}