using application.MVVM.Model;

namespace application.Abstraction.Interfaces;

public interface IAccountRepository
{
	public Task<DeleteModel> GetEntityIdAsync(Guid entityId);
	public Task MarkUserAsDeletedAsync(Guid userId);
	public Task MarkCompanyAsDeletedAsync(Guid companyId);
}