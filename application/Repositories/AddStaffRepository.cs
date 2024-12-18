using application.Abstraction;
using application.Abstraction.Interfaces;
using application.MVVM.Model;
using application.Utilities;

using Dapper;

namespace application.Repositories;

public class AddStaffRepository : IAddStaffRepository
{
	private readonly IDatabaseService _databaseService;

	public AddStaffRepository(IDatabaseService databaseService) => _databaseService = databaseService;

	public async Task<(Guid Id, string Name)> GetUser(Guid userId, string email)
	{
		return await RepositoryHelper.ExecuteWithErrorHandlingAsync(async dbConnection =>
		{
			const string query = @"SELECT id, CONCAT(firstname, ' ', secondname, ' ', thirdname) AS name 
                  FROM ""user""
                  WHERE email = @Email
                  AND id <> @UserId";

			var result = await dbConnection.QuerySingleOrDefaultAsync<(Guid Id, string Name)>(query, new { UserId = userId, Email = email });

			if (result.Id == null || result.Name == null || result.Id == userId)
			{
				throw new InvalidOperationException($"User with email '{email}' not found.");
			}

			return result;
		}, _databaseService);
	}


	public async Task<Guid> InsertStaff(Guid entityId, EntityManagerModel managerModel)
	{
		return await RepositoryHelper.ExecuteWithErrorHandlingAsync(async dbConnection =>
		{
			string query = $@"INSERT INTO ENTITY_MANAGERS
                        (ID, Entity_ID, User_ID, Access)
                        VALUES (
                         @{nameof(EntityManagerModel.Id)},
                         @Entity_Id,
                         @{nameof(EntityManagerModel.User_ID)},
                         @{nameof(EntityManagerModel.Access)}
                        )
                        RETURNING ID";

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
}