using application.MVVM.Model;

namespace application.Abstraction.Interfaces;

public interface IStaffRepository
{
	public Task<IEnumerable<StaffMember>> GetStaffByEntityIdAsync(Guid entityId, string searchQuery = "");
	public Task<bool> DeleteStaffMemberAsync(Guid staffId);
}