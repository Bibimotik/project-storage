using System.Data;
using System.Diagnostics;

using application.Abstraction;
using application.Services;

using Dapper;

namespace application.Repository;

public class StatusRepository : IStatusRepository
{
	private readonly IDatabaseService _databaseService;

	public StatusRepository(IDatabaseService databaseService) => _databaseService = databaseService;

	public IEnumerable<StatusModel> GetAllStatus()
	{
		using (IDbConnection dbConnection = _databaseService.CreateConnection())
		{
			Debug.WriteLine("GetAllStatus!!!!!!!");
			string query = "SELECT * FROM status";
			return dbConnection.Query<StatusModel>(query).ToList();
		}
	}
}
