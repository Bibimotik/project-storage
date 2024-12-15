using System.Data;
using System.Windows;

using application.Abstraction;

using Npgsql;

namespace application.Utilities;

public static class RepositoryHelper
{

	public static async Task<T> ExecuteWithErrorHandlingAsync<T>(Func<IDbConnection, Task<T>> func, IDatabaseService databaseService)
	{
		IDbConnection dbConnection = null;
		try
		{
			dbConnection = databaseService.CreateConnection();

			if (dbConnection is NpgsqlConnection npgsqlConnection)
				await npgsqlConnection.OpenAsync();
			else
				dbConnection.Open();

			return await func(dbConnection);
		}
		catch (Exception ex)
		{
			MessageBox.Show($"Ошибка при выполнении операции с базой данных: {ex.Message}");
			throw;
		}
		finally
		{
			dbConnection?.Dispose();
		}
	}
}