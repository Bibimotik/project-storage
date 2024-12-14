using application.MVVM.Model;

namespace application.Abstraction.Interfaces;

public interface ITablesRepository
{
	public interface ITablesRepository
	{
		Task<IList<EntityModel>> GetCompanies();
		Task<IList<EntityTableModel>> GetEntities();
		Task<IList<EntityManagerModel>> GetEntitiyManagers();
		Task<IList<EntityProductModel>> GetEntitiyProductOrders();
		Task<IList<EntityProductModel>> GetEntitiyProducts();
		Task<IList<EntityStorageModel>> GetEntitiyStorages();
		Task<IList<OrderModel>> GetOrders();
		Task<IList<SupportModel>> GetSupports();
		Task<IList<EntityModel>> GetUsers();
	}
}