using application.Abstraction;
using application.Abstraction.Interfaces;
using application.MVVM.Model;
using application.Utilities;

using Dapper;

namespace application.Repositories;

public class SaleRepository : ISaleRepository
{
	private readonly IDatabaseService _databaseService;
	public SaleRepository(IDatabaseService databaseService) => _databaseService = databaseService;
	
	public async Task<IEnumerable<OrderModel>> GetOrdersByEntityIdAsync(Guid entityId, string searchQuery = "", string orderBy = "")
	{
		return await RepositoryHelper.ExecuteWithErrorHandlingAsync(async dbConnection =>
		{
			string query = @"
	            SELECT 
			    id, 
			    entity_id AS EntityId, 
			    entity_managers_id AS EntityManagersId, 
			    inn, 
			    kpp, 
			    ogrn, 
			    fullname, 
			    address, 
			    payment_account AS PaymentAccount, 
			    tocor_account AS ToCorAccount, 
			    tobik AS ToBIK, 
			    tobank AS ToBank, 
			    fromcor_account AS FromCorAccount, 
			    frombik AS FromBIK, 
			    frombank AS FromBank, 
			    plan_date_shipment AS PlanDateShipment, 
			    shipping_address AS ShippingAddress, 
			    application_date AS ApplicationDate, 
			    delivery_point AS DeliveryPoint, 
			    delivery_address AS DeliveryAddress, 
			    plan_date_receipt AS PlanDateReceipt, 
			    transporterfullname AS TransporterFullName, 
			    transportershortname AS TransporterShortName, 
			    comment, 
			    vat, 
			    is_deleted AS IsDeleted
			FROM 
			    ""order""
			WHERE 
			    entity_id = @EntityId
			    AND fullname LIKE @Substring
			    AND is_deleted = false";

			if (orderBy == "ASC")
			{
				query += " ORDER BY fullname ASC";
			}
			else if (orderBy == "DESC")
			{
				query += " ORDER BY fullname DESC";
			}

			var result = await dbConnection.QueryAsync<OrderModel>(query, new
			{
				EntityId = entityId,
				Substring = $"%{searchQuery}%"
			});

			return result;
		}, _databaseService);
	}
	
	public async Task<bool> MarkSaleAsDeletedAsync(Guid saleId)
	{
		return await RepositoryHelper.ExecuteWithErrorHandlingAsync(async dbConnection =>
		{
			string query = @"UPDATE ""order""
                             SET is_deleted = true
                             WHERE id = @OrderId";

			var result = await dbConnection.ExecuteAsync(query, new { OrderId = saleId });

			return result > 0;
		}, _databaseService);
	}
	
	public async Task<IEnumerable<OrderModel>> GetOrdersByOrderIdAsync(Guid orderId)
	{
		return await RepositoryHelper.ExecuteWithErrorHandlingAsync(async dbConnection =>
		{
			string query = @"
	            SELECT 
			    id, 
			    entity_id AS EntityId, 
			    entity_managers_id AS EntityManagersId, 
			    inn, 
			    kpp, 
			    ogrn, 
			    fullname, 
			    address, 
			    payment_account AS PaymentAccount, 
			    tocor_account AS ToCorAccount, 
			    tobik AS ToBIK, 
			    tobank AS ToBank, 
			    fromcor_account AS FromCorAccount, 
			    frombik AS FromBIK, 
			    frombank AS FromBank, 
			    plan_date_shipment AS PlanDateShipment, 
			    shipping_address AS ShippingAddress, 
			    application_date AS ApplicationDate, 
			    delivery_point AS DeliveryPoint, 
			    delivery_address AS DeliveryAddress, 
			    plan_date_receipt AS PlanDateReceipt, 
			    transporterfullname AS TransporterFullName, 
			    transportershortname AS TransporterShortName, 
			    comment, 
			    vat, 
			    is_deleted AS IsDeleted
			FROM 
			    ""order""
			WHERE 
			    id = @OrderId
			    AND is_deleted = false";

			var result = await dbConnection.QueryAsync<OrderModel>(query, new
			{
				OrderId = orderId
			});

			return result;
		}, _databaseService);
	}
}