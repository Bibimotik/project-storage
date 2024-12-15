
namespace application.Abstraction.Interfaces;

public interface ITablesRepository
{
	Task DeleteData<T>(EntityAbstraction.TableNames tableName, Guid id);
	Task DeleteEntity(Guid id);
	Task<IList<T>> GetData<T>(EntityAbstraction.TableNames tableName);
}