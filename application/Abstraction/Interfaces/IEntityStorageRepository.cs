using application.MVVM.Model;

namespace application.Abstraction.Interfaces;

public interface IEntityStorageRepository
{
	public Task<Guid> InsertEntityStorage(Guid entityId, EntityStorageModel storageModel);
}