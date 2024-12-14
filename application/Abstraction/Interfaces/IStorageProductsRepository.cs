using application.MVVM.Model;

namespace application.Abstraction.Interfaces;

public interface IStorageProductsRepository
{
	public Task<IEnumerable<ProductDataResult>> GetProductsDataAsync(Guid storageId);
}