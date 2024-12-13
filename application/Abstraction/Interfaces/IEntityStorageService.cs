using application.MVVM.Model;

namespace application.Abstraction.Interfaces;

public interface IEntityStorageService
{
	public  Task<Guid> Insert(Guid id, EntityStorageModel model);
}