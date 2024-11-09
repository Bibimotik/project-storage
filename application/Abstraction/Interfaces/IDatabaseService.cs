using System.Data;

namespace application.Abstraction;

public interface IDatabaseService
{
	public IDbConnection CreateConnection();
}
