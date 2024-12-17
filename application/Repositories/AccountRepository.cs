using application.Abstraction;
using application.Abstraction.Interfaces;
using application.MVVM.Model;
using application.Utilities;
using Dapper;

namespace application.Repositories;

public class AccountRepository : IAccountRepository
{
	private readonly IDatabaseService _databaseService;

	public AccountRepository(IDatabaseService databaseService) => _databaseService = databaseService;

	public async Task<DeleteModel> GetEntityIdAsync(Guid entityId)
	{
		return await RepositoryHelper.ExecuteWithErrorHandlingAsync(async dbConnection =>
		{
			string query = @"
                SELECT type, type_id
                FROM entity
                WHERE id = @EntityId";

			var result = await dbConnection.QueryFirstOrDefaultAsync<DeleteModel>(
				query,
				new { EntityId = entityId });

			return result;
		}, _databaseService);
	}
	
	public async Task MarkUserAsDeletedAsync(Guid userId)
	{
		await RepositoryHelper.ExecuteWithErrorHandlingAsync(async dbConnection =>
		{
			string updateUserQuery = @"
                UPDATE ""user""
                SET is_deleted = true
                WHERE id = @userId";

			var result = await dbConnection.ExecuteAsync(updateUserQuery, new { userId });

			return result;
		}, _databaseService);
	}

	public async Task MarkCompanyAsDeletedAsync(Guid companyId)
	{
		await RepositoryHelper.ExecuteWithErrorHandlingAsync(async dbConnection =>
		{
			string updateCompanyQuery = @"
                UPDATE company
                SET is_deleted = true
                WHERE id = @companyId";
			
			var result = await dbConnection.ExecuteAsync(updateCompanyQuery, new { companyId });

			return result;
		}, _databaseService);
	}
}