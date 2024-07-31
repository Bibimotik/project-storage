using application.Services;

namespace application.Abstraction;

public  interface IStatusRepository
{
	public IEnumerable<StatusModel> GetAllStatus();
}
