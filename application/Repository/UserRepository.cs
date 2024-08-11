using System.Diagnostics;

using application.Abstraction;
using application.MVVM.Model;
using application.Utilities;

using Dapper;

namespace application.Repository;

public class UserRepository : IUserRepository
{
	private readonly IDatabaseService _databaseService;

	public UserRepository(IDatabaseService databaseService) => _databaseService = databaseService;

	public async Task<EntityModel> GetUserLogin(string email)
	{
		return await RepositoryHelper.ExecuteWithErrorHandling(async dbConnection =>
		{
			Debug.WriteLine("GetUser!!!!!!!");

			string query = $@"SELECT 
				email as {nameof(EntityModel.Email)}, 
				password as {nameof(EntityModel.Password)} 
				FROM ""user"" 
				where email = @{nameof(EntityModel.Email)}";
			return await dbConnection.QuerySingleAsync<EntityModel>(query, new { Email = email });
		}, _databaseService);
	}

	public async Task<Guid> UserRegistration(EntityModel entity)
	{
		return await RepositoryHelper.ExecuteWithErrorHandling(async dbConnection =>
		{
			Debug.WriteLine("UserRegistration!!!!!!!");

			// TODO - добавить проверки на неповторяющийся email

			string query = $@"INSERT into ""user"" 
				(user_id, nickname, firstname, secondname, thirdname, phone, email, password, logo, is_deleted)
				values (
				@{nameof(EntityModel.Id)},
				NULL,
				@{nameof(EntityModel.FirstName)},
				@{nameof(EntityModel.SecondName)},
				@{nameof(EntityModel.ThirdName)},
				@{nameof(EntityModel.Phone)},
				@{nameof(EntityModel.Email)},
				@{nameof(EntityModel.Password)},
				NULL,
				FALSE)
				returning user_id";
			return await dbConnection.QuerySingleAsync<Guid>(query, entity);
		}, _databaseService);
	}
}
