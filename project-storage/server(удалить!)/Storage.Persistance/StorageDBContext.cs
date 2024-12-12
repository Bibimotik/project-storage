using System.Data;

using Npgsql;

using Storage.Domain.Interfaces;

namespace Storage.Persistance;

public class StorageDBContext(string connectionString) : IStorageDBContext
{
	private readonly string _connectionString = connectionString;

	public async Task<IDbConnection> CreateConnection(CancellationToken cancellationToken)
	{
		var connection = new NpgsqlConnection(_connectionString);
		await connection.OpenAsync(cancellationToken);
		return connection;
	}
}
