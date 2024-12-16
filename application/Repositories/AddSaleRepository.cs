using application.Abstraction;
using application.Abstraction.Interfaces;
using application.MVVM.Model;
using application.Utilities;

using Dapper;

namespace application.Repositories;

public class AddSaleRepository : IAddSaleRepository
{
	private readonly IDatabaseService _databaseService;
	
	public AddSaleRepository(IDatabaseService databaseService) => _databaseService = databaseService;

	public async Task<Guid> InsertOrder(OrderModel orderModel)
	{
	    return await RepositoryHelper.ExecuteWithErrorHandlingAsync(async dbConnection =>
	    {
	        string query = $@"INSERT INTO orders (
	                                ID,
	                                Entity_ID,
	                                Entity_Managers_ID,
	                                INN,
	                                KPP,
	                                OGRN,
	                                FullName,
	                                Address,
	                                Payment_Account,
	                                toCor_Account,
	                                toBIK,
	                                toBank,
	                                fromCor_Account,
	                                fromBIK,
	                                fromBank,
	                                Plan_Date_Shipment,
	                                Shipping_Address,
	                                Application_Date,
	                                Delivery_Point,
	                                Delivery_Address,
	                                Plan_Date_Receipt,
	                                TransporterFullName,
	                                TransporterShortName,
	                                Comment,
	                                VAT,
	                                Is_Deleted
	                            )
	                            VALUES (
	                                @{nameof(OrderModel.ID)},
	                                @{nameof(OrderModel.Entity_ID)},
	                                @{nameof(OrderModel.Entity_Managers_ID)},
	                                @{nameof(OrderModel.INN)},
	                                @{nameof(OrderModel.KPP)},
	                                @{nameof(OrderModel.OGRN)},
	                                @{nameof(OrderModel.FullName)},
	                                @{nameof(OrderModel.Address)},
	                                @{nameof(OrderModel.Payment_Account)},
	                                @{nameof(OrderModel.ToCor_Account)},
	                                @{nameof(OrderModel.ToBIK)},
	                                @{nameof(OrderModel.ToBank)},
	                                @{nameof(OrderModel.FromCor_Account)},
	                                @{nameof(OrderModel.FromBIK)},
	                                @{nameof(OrderModel.FromBank)},
	                                @{nameof(OrderModel.Plan_Date_Shipment)},
	                                @{nameof(OrderModel.Shipping_Address)},
	                                @{nameof(OrderModel.Application_Date)},
	                                @{nameof(OrderModel.Delivery_Point)},
	                                @{nameof(OrderModel.Delivery_Address)},
	                                @{nameof(OrderModel.Plan_Date_Receipt)},
	                                @{nameof(OrderModel.TransporterFullName)},
	                                @{nameof(OrderModel.TransporterShortName)},
	                                @{nameof(OrderModel.Comment)},
	                                @{nameof(OrderModel.VAT)},
	                                FALSE
	                            )
	                            RETURNING ID";

	        var parameters = new
	        {
	            ID = orderModel.ID,
	            Entity_ID = orderModel.Entity_ID,
	            Entity_Managers_ID = orderModel.Entity_Managers_ID,
	            INN = orderModel.INN,
	            KPP = orderModel.KPP,
	            OGRN = orderModel.OGRN,
	            FullName = orderModel.FullName,
	            Address = orderModel.Address,
	            Payment_Account = orderModel.Payment_Account,
	            toCor_Account = orderModel.ToCor_Account,
	            toBIK = orderModel.ToBIK,
	            toBank = orderModel.ToBank,
	            fromCor_Account = orderModel.FromCor_Account,
	            fromBIK = orderModel.FromBIK,
	            fromBank = orderModel.FromBank,
	            Plan_Date_Shipment = orderModel.Plan_Date_Shipment,
	            Shipping_Address = orderModel.Shipping_Address,
	            Application_Date = orderModel.Application_Date,
	            Delivery_Point = orderModel.Delivery_Point,
	            Delivery_Address = orderModel.Delivery_Address,
	            Plan_Date_Receipt = orderModel.Plan_Date_Receipt,
	            TransporterFullName = orderModel.TransporterFullName,
	            TransporterShortName = orderModel.TransporterShortName,
	            Comment = orderModel.Comment,
	            VAT = orderModel.VAT
	        };

	        Guid orderId = await dbConnection.QuerySingleAsync<Guid>(query, parameters);

	        return orderId;
	    }, _databaseService);
	}
	
	public async Task<Guid> GetStorage(Guid entityId, string storage)
	{
		return await RepositoryHelper.ExecuteWithErrorHandlingAsync(async dbConnection =>
		{
			const string query = @"SELECT id
                               FROM ENTITY_STORAGE
                               WHERE point = @Storage
                               AND entity_id = @Entity_Id";

			var result = await dbConnection.QuerySingleOrDefaultAsync<Guid>(query, new { Entity_Id = entityId, Storage = storage });

			if (result == Guid.Empty)
			{
				throw new InvalidOperationException($"Storage with point '{storage}' not found.");
			}

			return result;
		}, _databaseService);
	}
	
	public async Task<IEnumerable<ProductDataResult>> GetProductsDataAsync(Guid storageId)
	{
		return await RepositoryHelper.ExecuteWithErrorHandlingAsync(async dbConnection =>
		{
			const string query = @"
				SELECT id, code, title, unit, price, image, available_for_shipment, party, implementation_period, expiration_date
				FROM PRODUCT
				WHERE entity_storage_id = @StorageId";
			
			var result = await dbConnection.QueryAsync<ProductDataResult>(query, new { StorageId = storageId });

			return result;
		}, _databaseService);
	}
}