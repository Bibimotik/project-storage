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
	
	public async Task<IEnumerable<StaffMember>> GetStaffByEntityIdAsync(Guid entityId)
	{
		const string query = @"
            SELECT 
                staff.id, 
                staff.access, 
                u.firstname, 
                u.secondname, 
                u.thirdname, 
                u.email
            FROM (
                SELECT *
                FROM entity_managers
                WHERE entity_id = @EntityId
            ) staff
            INNER JOIN ""user"" u
            ON staff.user_id = u.id";

		return await RepositoryHelper.ExecuteWithErrorHandlingAsync(async dbConnection =>
		{
			return await dbConnection.QueryAsync<StaffMember>(query, new { EntityId = entityId });
		}, _databaseService);
	}
}