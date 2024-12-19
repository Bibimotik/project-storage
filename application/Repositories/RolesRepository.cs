using application.Abstraction;
using application.Abstraction.Interfaces;
using application.MVVM.Model;
using application.Utilities;

using Dapper;

namespace application.Repositories;

public class RolesRepository : IRolesRepository
{
	private readonly IDatabaseService _databaseService;

	public RolesRepository(IDatabaseService databaseService) => _databaseService = databaseService;

	public async Task<Guid> GetCompanyId(Guid userId)
	{
		return await RepositoryHelper.ExecuteWithErrorHandlingAsync(async dbConnection =>
		{
			const string query = @"select entity_id
				from entity_managers
				where user_id = @UserId";

			return await dbConnection.QueryFirstAsync<Guid>(query, new { UserId = userId });
		}, _databaseService);
	}
	public async Task<Guid> GetUserEntityId(Guid userId)
	{
		return await RepositoryHelper.ExecuteWithErrorHandlingAsync(async dbConnection =>
		{
			const string query = @"select id
				from entity
				where type_id = @UserId";

			return await dbConnection.QuerySingleAsync<Guid>(query, new { UserId = userId });
		}, _databaseService);
	}

	public async Task<IEnumerable<RoleDataResult>> GetEntityDataAsync(Guid userId)
	{
		return await RepositoryHelper.ExecuteWithErrorHandlingAsync(async dbConnection =>
		{
			const string query = @"
                WITH entitydata AS (
                    SELECT *
                    FROM (
                        SELECT entity_id, access
                        FROM entity_managers
                        WHERE user_id = @UserId
                    ) entitydata, entity
                    WHERE entitydata.entity_id = entity.id
                ),
                user_query AS (
                    SELECT 
						entitydata.id AS id,
                        u.firstname AS first,
                        u.secondname AS second,
                        u.email AS third,
                        u.logo AS logo,
                        entitydata.access
                    FROM entitydata
                    JOIN ""user"" u ON u.id = entitydata.type_id
                    WHERE entitydata.type = 'User'
                ),
                company_query AS (
                    SELECT 
						entitydata.id AS id,
                        c.shortname AS first,
                        c.inn AS second,
                        c.kpp AS third,
                        c.logo AS logo,
                        entitydata.access
                    FROM entitydata
                    JOIN company c ON c.id = entitydata.type_id
                    WHERE entitydata.type = 'Company'
                )
                SELECT * FROM user_query
                UNION ALL
                SELECT * FROM company_query";

			var result = await dbConnection.QueryAsync<RoleDataResult>(query, new { UserId = userId });

			return result;
		}, _databaseService);
	}
}
