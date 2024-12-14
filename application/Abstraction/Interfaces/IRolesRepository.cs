using application.MVVM.Model;

namespace application.Abstraction.Interfaces;

public interface IRolesRepository
{
	public Task<IEnumerable<RoleDataResult>> GetEntityDataAsync(Guid userId);
}