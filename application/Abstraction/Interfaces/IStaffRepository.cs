using application.MVVM.Model;

namespace application.Abstraction.Interfaces;

public interface IStaffRepository
{
	public Task<IEnumerable<StaffMember>> GetStaffByEntityIdAsync(Guid entityId, string searchQuery = "");
	public Task<bool> DeleteStaffMemberAsync(Guid staffId);
	public Task<Guid> InsertStaff(Guid entityId, EntityManagerModel managerModel);
	Task<bool> CheckIfStaffExists(Guid entityId, Guid userId, string access);
}