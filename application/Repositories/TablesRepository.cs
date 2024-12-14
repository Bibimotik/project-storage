using application.Abstraction;
using application.Abstraction.Interfaces;
using application.MVVM.Model;
using application.Utilities;

using Dapper;

namespace application.Repositories;

public class TablesRepository : ITablesRepository
{
	private readonly IDatabaseService _databaseService;

	public TablesRepository(IDatabaseService databaseService) => _databaseService = databaseService;

	public async Task<IList<EntityModel>> GetCompanies()
	{
		return await RepositoryHelper.ExecuteWithErrorHandlingAsync(async dbConnection =>
		{
			var query = "select * from company";

			var result = await dbConnection.QueryAsync<EntityModel>(query);
			return result.ToList();

		}, _databaseService);
	}

	public async Task<IList<EntityTableModel>> GetEntities()
	{
		return await RepositoryHelper.ExecuteWithErrorHandlingAsync(async dbConnection =>
		{
			var query = "select * from entity";

			var result = await dbConnection.QueryAsync<EntityTableModel>(query);
			return result.ToList();

		}, _databaseService);
	}

	public async Task<IList<EntityManagerModel>> GetEntitiyManagers()
	{
		return await RepositoryHelper.ExecuteWithErrorHandlingAsync(async dbConnection =>
		{
			var query = "select * from entity_managers";

			var result = await dbConnection.QueryAsync<EntityManagerModel>(query);
			return result.ToList();

		}, _databaseService);
	}

	public async Task<IList<EntityProductModel>> GetEntitiyProducts()
	{
		return await RepositoryHelper.ExecuteWithErrorHandlingAsync(async dbConnection =>
		{
			var query = "select * from entity_product";

			var result = await dbConnection.QueryAsync<EntityProductModel>(query);
			return result.ToList();

		}, _databaseService);
	}

	public async Task<IList<EntityProductModel>> GetEntitiyProductOrders()
	{
		return await RepositoryHelper.ExecuteWithErrorHandlingAsync(async dbConnection =>
		{
			var query = "select * from entity_product_order";

			var result = await dbConnection.QueryAsync<EntityProductModel>(query);
			return result.ToList();

		}, _databaseService);
	}

	public async Task<IList<EntityStorageModel>> GetEntitiyStorages()
	{
		return await RepositoryHelper.ExecuteWithErrorHandlingAsync(async dbConnection =>
		{
			var query = "select * from entity_storage";

			var result = await dbConnection.QueryAsync<EntityStorageModel>(query);
			return result.ToList();

		}, _databaseService);
	}

	public async Task<IList<OrderModel>> GetOrders()
	{
		return await RepositoryHelper.ExecuteWithErrorHandlingAsync(async dbConnection =>
		{
			var query = "select * from order";

			var result = await dbConnection.QueryAsync<OrderModel>(query);
			return result.ToList();

		}, _databaseService);
	}

	public async Task<IList<SupportModel>> GetSupports()
	{
		return await RepositoryHelper.ExecuteWithErrorHandlingAsync(async dbConnection =>
		{
			var query = "select * from support";

			var result = await dbConnection.QueryAsync<SupportModel>(query);
			return result.ToList();

		}, _databaseService);
	}

	public async Task<IList<EntityModel>> GetUsers()
	{
		return await RepositoryHelper.ExecuteWithErrorHandlingAsync(async dbConnection =>
		{
			var query = "select * from \"user\"";

			var result = await dbConnection.QueryAsync<EntityModel>(query);
			return result.ToList();

		}, _databaseService);
	}
}
