using application.Abstraction;
using application.Abstraction.Interfaces;
using application.MVVM.Model;
using application.Utilities;

using Dapper;

namespace application.Repositories;

public class StorageProductsRepository : IStorageProductsRepository
{
	private readonly IDatabaseService _databaseService;
	
	public StorageProductsRepository(IDatabaseService databaseService) => _databaseService = databaseService;

	public async Task<IEnumerable<ProductDataResult>> GetProductsDataAsync(Guid storageId, string searchQuery = "", string orderBy = "")
	{
		return await RepositoryHelper.ExecuteWithErrorHandlingAsync(async dbConnection =>
		{
			string query = @"
				SELECT 
					id, 
					code, 
					title, 
					unit, 
					price, 
					image, 
					available_for_shipment, 
					party, 
					implementation_period, 
					expiration_date
				FROM PRODUCT
				WHERE entity_storage_id = @StorageId
				AND title LIKE @Substring
				AND is_deleted = false";

			if (orderBy == "ASC")
			{
				query += " ORDER BY title ASC";
			}
			else if (orderBy == "DESC")
			{
				query += " ORDER BY title DESC";
			}

			var result = await dbConnection.QueryAsync<ProductDataResult>(query, new
			{
				StorageId = storageId,
				Substring = $"%{searchQuery}%"
			});

			return result;
		}, _databaseService);
	}
	
	public async Task<bool> MarkProductAsDeletedAsync(Guid productId)
	{
		return await RepositoryHelper.ExecuteWithErrorHandlingAsync(async dbConnection =>
		{
			string query = @"UPDATE product
                             SET is_deleted = true
                             WHERE id = @ProductId";

			var result = await dbConnection.ExecuteAsync(query, new { ProductId = productId });

			return result > 0;
		}, _databaseService);
	}
}