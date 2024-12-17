using application.MVVM.Model;

namespace application.Abstraction.Interfaces;

public interface IRolesRepository
{
	Task<IEnumerable<RoleDataResult>> GetCompanyId(Guid entityId);
	public Task<Guid> GetEntityDataAsync(Guid userId);
}