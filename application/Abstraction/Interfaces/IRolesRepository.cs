using application.MVVM.Model;

namespace application.Abstraction.Interfaces;

public interface IRolesRepository
{
	Task<Guid> GetCompanyId(Guid userId);
	public Task<IEnumerable<RoleDataResult>> GetEntityDataAsync(Guid userId);
	Task<Guid> GetUserEntityId(Guid userId);
}