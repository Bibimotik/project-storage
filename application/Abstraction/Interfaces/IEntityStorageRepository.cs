using application.MVVM.Model;

namespace application.Abstraction.Interfaces;

public interface IEntityStorageRepository
{
	Task<EntityStorageModel> GetEntityStorage(Guid storageId);
	Task<Guid> InsertEntityStorage(Guid entityId, EntityStorageModel storageModel);
	Task UpdateEntityStorage(Guid entityId, EntityStorageModel storageModel);
}