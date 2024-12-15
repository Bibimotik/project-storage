namespace application.Abstraction.Interfaces;

public interface ITablesRepository
{
	Task<IList<T>> GetData<T>(EntityAbstraction.TableNames tableName);
}