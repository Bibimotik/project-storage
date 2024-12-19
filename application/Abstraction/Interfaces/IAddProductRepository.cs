using application.MVVM.Model;

namespace application.Abstraction.Interfaces;

public interface IAddProductRepository
{
	Task<ProductModel> GetProduct(Guid productId);
	Task<Guid> InsertProduct(Guid entityId, ProductModel productModel);
	Task UpdateProduct(Guid entityId, ProductModel productModel);
}