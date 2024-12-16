using application.Abstraction;
using application.Abstraction.Interfaces;
using application.MVVM.Model;
using application.Utilities;

using Dapper;

namespace application.Repositories;

public class SaleRepository : ISaleRepository
{
	private readonly IDatabaseService _databaseService;
	public SaleRepository(IDatabaseService databaseService) => _databaseService = databaseService;
	
	public async Task<IEnumerable<OrderModel>> GetOrdersByEntityIdAsync(Guid entityId, string searchQuery = "", string orderBy = "")
	{
		return await RepositoryHelper.ExecuteWithErrorHandlingAsync(async dbConnection =>
		{
			string query = @"
	            SELECT 
	                id, 
	                entity_managers_id AS EntityManagersId, 
	                inn, 
	                kpp, 
	                fullname, 
	                address, 
	                application_date 
	            FROM ""order""
	            WHERE entity_id = @EntityId
	            AND fullname LIKE @Substring";

			// Добавление сортировки
			if (orderBy == "ASC")
			{
				query += " ORDER BY fullname ASC";
			}
			else if (orderBy == "DESC")
			{
				query += " ORDER BY fullname DESC";
			}

			var result = await dbConnection.QueryAsync<OrderModel>(query, new
			{
				EntityId = entityId,
				Substring = $"%{searchQuery}%"
			});

			return result;
		}, _databaseService);
	}
}