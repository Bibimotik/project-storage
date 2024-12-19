using application.MVVM.Model;

namespace application.Abstraction.Interfaces;

public interface IAccountRepository
{
	public Task MarkUserAsDeletedAsync(Guid userId);
	public Task MarkCompanyAsDeletedAsync(Guid companyId);
}