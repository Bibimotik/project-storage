using application.MVVM.Model;

namespace application.Abstraction.Interfaces;

public interface IAddStaffRepository
{
	Task<(Guid Id, string Name)> GetUser(Guid userId, string email);
	public Task<Guid> InsertStaff(Guid entityId, EntityManagerModel managerModel);
}