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
			{
				await npgsqlConnection.OpenAsync(); // Асинхронно открываем соединение, если это NpgsqlConnection
			}
			else
			{
				dbConnection.Open(); // Синхронное открытие для других реализаций
			}

			return await func(dbConnection); // Выполняем переданный метод
		}
		catch (Exception ex)
		{
			MessageBox.Show($"Ошибка при выполнении операции с базой данных: {ex.Message}");
			throw;
		}
		finally
		{
			dbConnection?.Dispose(); // Закрываем соединение в finally
		}
	}



	//public static T ExecuteWithErrorHandling<T>(Func<IDbConnection, T> func, IDatabaseService databaseService)
	//{
	//	try
	//	{
	//		using IDbConnection dbConnection = databaseService.CreateConnection();
	//		return func(dbConnection);
	//	}
	//	catch (Exception ex)
	//	{
	//		MessageBox.Show($"Ошибка при выполнении операции с базой данных: {ex.Message}");
	//		throw;
	//	}
	//}

	//public static async Task<T> ExecuteWithErrorHandlingAsync<T>(Func<IDbConnection, Task<T>> func, IDatabaseService databaseService)
	//{
	//	// Отказ от using для асинхронного метода
	//	IDbConnection dbConnection = null;

	//	try
	//	{
	//		dbConnection = databaseService.CreateConnection();
	//		await dbConnection.OpenAsync(); // Асинхронное открытие соединения
	//		return await func(dbConnection);
	//	}
	//	catch (Exception ex)
	//	{
	//		MessageBox.Show($"Ошибка при выполнении операции с базой данных: {ex.Message}");
	//		throw;
	//	}
	//	finally
	//	{
	//		dbConnection?.Dispose(); // Закрытие соединения вручную в finally
	//	}
	//}
}