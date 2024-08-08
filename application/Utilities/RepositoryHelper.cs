using System.Data;
using System.Windows;

using application.Abstraction;

namespace application.Utilities;

public static class RepositoryHelper
{
	public static T ExecuteWithErrorHandling<T>(Func<IDbConnection, T> func, IDatabaseService databaseService)
	{
		try
		{
			using IDbConnection dbConnection = databaseService.CreateConnection();
			return func(dbConnection);
		}
		catch (Exception ex)
		{
			// Логирование ошибки
			MessageBox.Show($"Ошибка при выполнении операции с базой данных: {ex.Message}");
			throw;
		}
	}

	// TODO - вместо этого
	//public LoginModel GetUserLogin(string email)
	//{
	//	try
	//	{
	//		using IDbConnection dbConnection = _databaseService.CreateConnection();
	//		Debug.WriteLine("GetUser!!!!!!!");

	//		string query = $@"SELECT 
	//			email as {nameof(LoginModel.Email)}, 
	//			password as {nameof(LoginModel.Password)} 
	//			FROM ""user"" 
	//			where email = @Email";
	//		return dbConnection.QuerySingle<LoginModel>(query, new { Email = email });
	//	}
	//	catch (Exception ex)
	//	{
	//		MessageBox.Show($"Ошибка при выполнении операции с базой данных: {ex.Message}");
	//		throw;
	//	}
	//}
}
