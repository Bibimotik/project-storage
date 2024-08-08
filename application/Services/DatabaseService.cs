using System.Data;

using application.Abstraction;

using Npgsql;

namespace application.Services
{
	public class DatabaseService : IDatabaseService
	{
		private readonly string _connectionString;

		public DatabaseService(string connectionString) => _connectionString = connectionString;

		public IDbConnection CreateConnection() => new NpgsqlConnection(_connectionString);
	}
}
