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
                u.logo
            FROM (
                SELECT *
                FROM entity_managers
                WHERE entity_id = @EntityId
            ) staff
            INNER JOIN ""user"" u
            ON staff.user_id = u.id
            AND u.email LIKE @Substring";

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
}