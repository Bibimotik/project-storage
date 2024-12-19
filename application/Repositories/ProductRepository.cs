using application.Abstraction;
using application.Abstraction.Interfaces;
using application.MVVM.Model;
using application.Utilities;

using Dapper;

namespace application.Repositories;

public class ProductRepository : IAddProductRepository
{
	private readonly IDatabaseService _databaseService;
	
	public ProductRepository(IDatabaseService databaseService) => _databaseService = databaseService;

	public async Task<ProductModel> GetProduct(Guid productId)
	{
		return await RepositoryHelper.ExecuteWithErrorHandlingAsync(async dbConnection =>
		{
			string query = $@"SELECT *
                FROM product 
                WHERE id = @ProductId";

			return await dbConnection.QuerySingleAsync<ProductModel>(query, new { ProductId = productId });
		}, _databaseService);
	}

	public async Task<Guid> InsertProduct(Guid entityId, ProductModel productModel)
	{
		return await RepositoryHelper.ExecuteWithErrorHandlingAsync(async dbConnection =>
		{
			string query = $@"INSERT INTO PRODUCT
                (ID, Entity_ID, Code, Title, Unit, Price, Image, Entity_Storage_Id, Available_For_Shipment, Party, Implementation_Period, Expiration_Date, Is_Deleted)
                VALUES (
                    @{nameof(ProductModel.Id)},
                    @{nameof(ProductModel.Entity_Id)},
                    @{nameof(ProductModel.Code)},
                    @{nameof(ProductModel.Title)},
                    @{nameof(ProductModel.Unit)},
                    @{nameof(ProductModel.Price)},
                    @{nameof(ProductModel.Image)},
                    @{nameof(ProductModel.Entity_Storage_ID)},
                    @{nameof(ProductModel.Available_For_Shipment)},
                    @{nameof(ProductModel.Party)},
                    @{nameof(ProductModel.Implementation_Period)},
                    @{nameof(ProductModel.Expiration_Date)},
                    FALSE)
                RETURNING ID";

			Guid productId = await dbConnection.QuerySingleAsync<Guid>(query, new
			{
				productModel.Id,
				productModel.Entity_Id,
				productModel.Code,
				productModel.Title,
				productModel.Unit,
				productModel.Price,
				productModel.Image,
				productModel.Entity_Storage_ID,
				productModel.Available_For_Shipment,
				productModel.Party,
				productModel.Implementation_Period,
				productModel.Expiration_Date
			});

			return productId;
		}, _databaseService);
	}

	public async Task UpdateProduct(Guid entityId, ProductModel productModel)
	{
		await RepositoryHelper.ExecuteWithErrorHandlingAsync(async dbConnection =>
		{
			string query = $@"UPDATE PRODUCT SET
                Code = @{nameof(ProductModel.Code)},
                Title = @{nameof(ProductModel.Title)},
                Unit = @{nameof(ProductModel.Unit)},
                Price = @{nameof(ProductModel.Price)},
                Logo = @{nameof(ProductModel.Image)},
                Entity_Storage_Id = @{nameof(ProductModel.Entity_Storage_ID)},
                Available_For_Shipment = @{nameof(ProductModel.Available_For_Shipment)},
                Party = @{nameof(ProductModel.Party)},
                Implementation_Period = @{nameof(ProductModel.Implementation_Period)},
                Expiration_Date = @{nameof(ProductModel.Expiration_Date)},
                Is_Deleted = FALSE
                WHERE ID = @{nameof(ProductModel.Id)} 
				AND Entity_ID = @{nameof(ProductModel.Entity_Id)}";

			await dbConnection.ExecuteAsync(query, new
			{
				productModel.Id,
				productModel.Entity_Id,
				productModel.Code,
				productModel.Title,
				productModel.Unit,
				productModel.Price,
				productModel.Image,
				productModel.Entity_Storage_ID,
				productModel.Available_For_Shipment,
				productModel.Party,
				productModel.Implementation_Period,
				productModel.Expiration_Date
			});

			return Task.CompletedTask;
		}, _databaseService);
	}
}