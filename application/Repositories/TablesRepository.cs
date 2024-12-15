using application.Abstraction;
using application.Abstraction.Interfaces;
using application.Utilities;

using Dapper;

using static application.Abstraction.EntityAbstraction;

namespace application.Repositories;

public class TablesRepository : ITablesRepository
{
	private readonly IDatabaseService _databaseService;

	public TablesRepository(IDatabaseService databaseService) => _databaseService = databaseService;

	public async Task<IList<T>> GetData<T>(TableNames tableName)
	{
		return await RepositoryHelper.ExecuteWithErrorHandlingAsync(async dbConnection =>
		{
			var query = $@"select * from ""{tableName.GetDescription()}""";

			var result = await dbConnection.QueryAsync<T>(query);
			return result.ToList();

		}, _databaseService);
	}
}
