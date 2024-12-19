using application.Abstraction;
using application.Abstraction.Interfaces;
using application.MVVM.Model;
using application.Utilities;

using Dapper;

namespace application.Repositories;

public class StaffRepository : IStaffRepository
{
	private readonly IDatabaseService _databaseService;
	public StaffRepository(IDatabaseService databaseService) => _databaseService = databaseService;
	
	public async Task<IEnumerable<StaffMember>> GetStaffByEntityIdAsync(Guid entityId, string searchQuery = "")
	{
		const string query = @"
            SELECT 
				staff.id, 
				staff.access, 
				u.firstname, 
				u.secondname, 
				u.thirdname, 
				u.email,
				u.logo AS Image
			FROM (
				SELECT *
				FROM entity_managers
				WHERE entity_id = @EntityId
			) staff
			INNER JOIN ""user"" u
			ON staff.user_id = u.id
			AND u.email LIKE @Substring
			";

		return await RepositoryHelper.ExecuteWithErrorHandlingAsync(async dbConnection =>
		{
			return await dbConnection.QueryAsync<StaffMember>(query, new { EntityId = entityId, Substring = $"%{searchQuery}%" });
		}, _databaseService);
	}
	
	public async Task<bool> DeleteStaffMemberAsync(Guid staffId)
	{
		const string query = @"
        DELETE FROM entity_managers
        WHERE id = @StaffId";

		var result = await RepositoryHelper.ExecuteWithErrorHandlingAsync(async dbConnection =>
		{
			var affectedRows = await dbConnection.ExecuteAsync(query, new { StaffId = staffId });
			return affectedRows > 0;
		}, _databaseService);

		return result;
	}

	public async Task<Guid> InsertStaff(Guid entityId, EntityManagerModel managerModel)
	{
		return await RepositoryHelper.ExecuteWithErrorHandlingAsync(async dbConnection =>
		{
			string query = $@"
				WITH NewRecord AS (
					SELECT 
						@{nameof(EntityManagerModel.Id)}::UUID AS Id,
						@Entity_Id AS Entity_Id,
						@{nameof(EntityManagerModel.User_ID)} AS User_ID,
						@{nameof(EntityManagerModel.Access)} AS Access
					WHERE NOT EXISTS (
						SELECT 1 
						FROM ENTITY_MANAGERS 
						WHERE Entity_ID = @Entity_Id 
						  AND User_ID = @{nameof(EntityManagerModel.User_ID)} 
						  AND Access = @{nameof(EntityManagerModel.Access)}
					)
				)
				INSERT INTO ENTITY_MANAGERS (ID, Entity_ID, User_ID, Access)
				SELECT Id, Entity_Id, User_ID, Access 
				FROM NewRecord
				RETURNING ID;
			";

			Guid staffId = await dbConnection.QuerySingleAsync<Guid>(query, new
			{
				managerModel.Id,
				Entity_Id = entityId,
				managerModel.User_ID,
				managerModel.Access
			});

			return staffId;
		}, _databaseService);
	}

	public async Task<bool> CheckIfStaffExists(Guid entityId, Guid userId, string access)
	{
		return await RepositoryHelper.ExecuteWithErrorHandlingAsync(async dbConnection =>
		{
			string query = @"
            SELECT 1 
            FROM ENTITY_MANAGERS
            WHERE Entity_ID = @EntityId 
              AND User_ID = @UserId
              AND Access = @Access
        ";

			// ¬ыполн€ем запрос и провер€ем, существует ли запись
			var result = await dbConnection.QuerySingleOrDefaultAsync<int?>(query, new
			{
				EntityId = entityId,
				UserId = userId,
				Access = access
			});

			return result.HasValue;
		}, _databaseService);
	}

}