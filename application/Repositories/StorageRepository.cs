using application.Abstraction;
using application.Abstraction.Interfaces;
using application.MVVM.Model;
using application.Utilities;

using Dapper;

namespace application.Repositories;

public class StorageRepository : IStorageRepository
{
	private readonly IDatabaseService _databaseService;
	
	public StorageRepository(IDatabaseService databaseService) => _databaseService = databaseService;

	public async Task<IEnumerable<StorageDataResult>> GetStorageDataAsync(Guid entityId)
	{
		return await RepositoryHelper.ExecuteWithErrorHandlingAsync(async dbConnection =>
		{
			const string query = @"
				SELECT id, point, country, city, address, index
				FROM entity_storage
				WHERE entity_id = @EntityId";
			
			var result = await dbConnection.QueryAsync<StorageDataResult>(query, new { EntityId = entityId });

			return result;
		}, _databaseService);
	}
}