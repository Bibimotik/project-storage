using application.MVVM.Model;

namespace application.Abstraction.Interfaces;

public interface IStorageRepository
{
	public Task<IEnumerable<StorageDataResult>> GetStorageDataAsync(Guid entityId, string searchQuery = "",
		string orderBy = "");

	public Task<bool> MarkStorageAsDeletedAsync(Guid storageId);
}