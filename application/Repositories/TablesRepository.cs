using System.Diagnostics;

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

			Debug.WriteLine(typeof(T));

			var result = await dbConnection.QueryAsync<T>(query);
			return result.ToList();

		}, _databaseService);
	}

	public async Task DeleteData<T>(TableNames tableName, Guid id)
	{
		await RepositoryHelper.ExecuteWithErrorHandlingAsync(async dbConnection =>
		{
			var query = $@"delete 
				from ""{tableName.GetDescription()}""
				where id = @Id";

			await dbConnection.ExecuteAsync(query, new { Id = id });

			return Task.CompletedTask;

		}, _databaseService);
	}

	public async Task DeleteEntity(Guid id)
	{
		await RepositoryHelper.ExecuteWithErrorHandlingAsync(async dbConnection =>
		{
			var query = $@"delete 
				from ""{TableNames.Entity.GetDescription()}""
				where type_id = @Id";

			await dbConnection.ExecuteAsync(query, new { Id = id });

			return Task.CompletedTask;

		}, _databaseService);
	}
}
