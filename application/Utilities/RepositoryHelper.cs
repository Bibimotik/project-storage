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
			MessageBox.Show($"Ошибка при выполнении операции с базой данных: {ex.Message}");
			throw;
		}
	}

	public static async Task<T> ExecuteWithErrorHandlingAsync<T>(Func<IDbConnection, Task<T>> func, IDatabaseService databaseService)
	{
		try
		{
			using IDbConnection dbConnection = databaseService.CreateConnection();
			return await func(dbConnection);
		}
		catch (Exception ex)
		{
			MessageBox.Show($"Ошибка при выполнении операции с базой данных: {ex.Message}");
			throw;
		}
	}
}