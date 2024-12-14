using application.MVVM.Model;

namespace application.Abstraction.Interfaces;

public interface IAddStaffRepository
{
	public Task<(Guid Id, string Name)> GetUser(string email);
	public Task<Guid> InsertStaff(Guid entityId, EntityManagerModel managerModel);
}