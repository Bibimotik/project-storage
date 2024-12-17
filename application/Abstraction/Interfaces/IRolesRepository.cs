using application.MVVM.Model;

namespace application.Abstraction.Interfaces;

public interface IRolesRepository
{
	//Task<Guid> GetCompanyId(Guid entityId);
	public Task<IEnumerable<RoleDataResult>> GetEntityDataAsync(Guid userId);
}