using System.Data;

namespace Storage.Domain.Interfaces;

public interface IStorageDBContext
{
	public Task<IDbConnection> CreateConnection(CancellationToken cancellationToken);
}
