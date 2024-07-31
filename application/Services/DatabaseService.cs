using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Dapper;

using Npgsql;

namespace application.Services
{
	class DatabaseService
    {
		private readonly string _connectionString;

		public DatabaseService(string connectionString) => _connectionString = connectionString;

		private IDbConnection CreateConnection() => new NpgsqlConnection(_connectionString);

		public IEnumerable<StatusModel> GetAllStatus()
		{
			using (IDbConnection dbConnection = CreateConnection())
			{
				Debug.WriteLine("GetAllStatus!!!!!!!");
				string query = "SELECT * FROM status";
				return dbConnection.Query<StatusModel>(query).ToList();
			}
		}

		//public void AddCompanies(Status user)
		//{
		//	using (IDbConnection dbConnection = CreateConnection())
		//	{
		//		string query = "INSERT INTO company (Username, Password) VALUES (@Username, @Password)";
		//		dbConnection.Execute(query, user);
		//	}
		//}
	}
}
