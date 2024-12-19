using application.MVVM.Model;

namespace application.Abstraction.Interfaces;

public interface ISupportRepository
{
	public Task<Guid> SentToSupport(SupportModel supportModel);
}