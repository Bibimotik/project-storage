using application.MVVM.Model;

namespace application.Abstraction.Interfaces;

public interface IAddProductRepository
{
	public Task<Guid> InsertProduct(Guid entityId, ProductModel productModel);
}