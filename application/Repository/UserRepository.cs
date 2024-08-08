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

	public LoginModel GetUserLogin(string email)
	{
		return RepositoryHelper.ExecuteWithErrorHandling(dbConnection =>
		{
			Debug.WriteLine("GetUser!!!!!!!");

			string query = $@"SELECT 
				email as {nameof(LoginModel.Email)}, 
				password as {nameof(LoginModel.Password)} 
				FROM ""user"" 
				where email = @Email";
			return dbConnection.QuerySingle<LoginModel>(query, new { Email = email });
		}, _databaseService);
	}
}
