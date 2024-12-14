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

	public async Task<IEnumerable<ProductDataResult>> GetProductsDataAsync(Guid storageId)
	{
		return await RepositoryHelper.ExecuteWithErrorHandlingAsync(async dbConnection =>
		{
			const string query = @"
				SELECT id, code, title, unit, price, image, available_for_shipment, party, implementation_period, expiration_date
				FROM PRODUCT
				WHERE entity_storage_id = @StorageId";
			
			var result = await dbConnection.QueryAsync<ProductDataResult>(query, new { StorageId = storageId });

			return result;
		}, _databaseService);
	}
}