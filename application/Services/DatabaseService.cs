using System.Data;
using System.Windows;

using application.Abstraction;

using Npgsql;

namespace application.Services;

public class DatabaseService : IDatabaseService
{
	private readonly string _connectionString;

	public DatabaseService(string connectionString)
	{
		if (string.IsNullOrEmpty(connectionString))
			MessageBox.Show("Отсутсвует подключение к бд");

		_connectionString = connectionString;
	}

	public IDbConnection CreateConnection() => new NpgsqlConnection(_connectionString);
}