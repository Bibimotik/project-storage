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

	public async Task<IEnumerable<StorageDataResult>> GetStorageDataAsync(Guid entityId, string searchQuery = "", string orderBy = "")
	{
		return await RepositoryHelper.ExecuteWithErrorHandlingAsync(async dbConnection =>
		{
			string query = @"SELECT 
			id, point, country, city, address, index
            FROM entity_storage
            WHERE entity_id = @EntityId
            AND point LIKE @Substring
            AND is_deleted = false";

			if (orderBy == "ASC")
			{
				query += " ORDER BY point ASC";
			}
			else if (orderBy == "DESC")
			{
				query += " ORDER BY point DESC";
			}

			var result = await dbConnection.QueryAsync<StorageDataResult>(query, new
			{
				EntityId = entityId,
				Substring = $"%{searchQuery}%"
			});

			return result;
		}, _databaseService);
	}
	
	public async Task<bool> MarkStorageAsDeletedAsync(Guid storageId)
	{
		return await RepositoryHelper.ExecuteWithErrorHandlingAsync(async dbConnection =>
		{
			string query = @"UPDATE entity_storage
                             SET is_deleted = true
                             WHERE id = @StorageId";

			var result = await dbConnection.ExecuteAsync(query, new { StorageId = storageId });

			return result > 0;
		}, _databaseService);
	}
}