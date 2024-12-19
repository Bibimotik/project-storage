using System.Diagnostics;

using application.Abstraction;
using application.Abstraction.Interfaces;
using application.MVVM.Model;
using application.Utilities;

using Dapper;

using static application.Abstraction.EntityAbstraction;

namespace application.Repositories;

public class TablesRepository : ITablesRepository
{
	private readonly IDatabaseService _databaseService;

	public TablesRepository(IDatabaseService databaseService) => _databaseService = databaseService;

	public async Task<IList<T>> GetData<T>(TableNames tableName)
	{
		return await RepositoryHelper.ExecuteWithErrorHandlingAsync(async dbConnection =>
		{
			var query = $@"select * from ""{tableName.GetDescription()}""";

			Debug.WriteLine(typeof(T));

			var result = await dbConnection.QueryAsync<T>(query);
			return result.ToList();

		}, _databaseService);
	}

	public async Task UpdateData(TableNames tableName, EntityModel updateValues)
	{
		await RepositoryHelper.ExecuteWithErrorHandlingAsync(async dbConnection =>
		{
			// Формирование SQL-запроса
			var query = $@"
        UPDATE ""{tableName.GetDescription()}"" 
        SET 
            firstname = @{nameof(EntityModel.FirstName)},	
            secondname = @{nameof(EntityModel.SecondName)},
            thirdname = @{nameof(EntityModel.ThirdName)},
            phone = @{nameof(EntityModel.Phone)},
            email = @{nameof(EntityModel.Email)},
            ""password"" = @{nameof(EntityModel.Password)},
            logo = @{nameof(EntityModel.Logo)},
            is_deleted = false
        WHERE id = @{nameof(EntityModel.Id)}";

			// Выполнение SQL-запроса
			var parameters = new
			{
				updateValues.FirstName,
				updateValues.SecondName,
				updateValues.ThirdName,
				updateValues.Phone,
				updateValues.Email,
				updateValues.Password,
				updateValues.Logo,
				updateValues.Id
			};

			await dbConnection.ExecuteAsync(query, parameters);

			return Task.CompletedTask;

		}, _databaseService);
	}

	public async Task UpdateData(TableNames tableName, EntityProductOrderModel updateValues)
	{
		await RepositoryHelper.ExecuteWithErrorHandlingAsync(async dbConnection =>
		{
			// Формирование SQL-запроса
			var query = $@"
            UPDATE ""{tableName.GetDescription()}"" 
            SET 
                product_id = @{nameof(EntityProductOrderModel.Product_ID)},
                order_id = @{nameof(EntityProductOrderModel.Order_ID)},
                count = @{nameof(EntityProductOrderModel.Count)}
            WHERE id = @{nameof(EntityProductOrderModel.ID)}";

			// Выполнение SQL-запроса
			await dbConnection.ExecuteAsync(query, updateValues);

			return Task.CompletedTask;

		}, _databaseService);
	}

	public async Task UpdateCompany(EntityModel updateValues)
	{
		await RepositoryHelper.ExecuteWithErrorHandlingAsync(async dbConnection =>
		{
			var query = $@"
            UPDATE public.company
            SET 
                inn = @{nameof(EntityModel.INN)},
                kpp = @{nameof(EntityModel.KPP)},
                ogrn = @{nameof(EntityModel.OGRN)},
                fullname = @{nameof(EntityModel.FullName)},
                shortname = @{nameof(EntityModel.ShortName)},
                email = @{nameof(EntityModel.Email)},
                ""password"" = @{nameof(EntityModel.Password)},
                legal_address = @{nameof(EntityModel.LegalAddress)},
                postal_address = @{nameof(EntityModel.PostalAddress)},
                director = @{nameof(EntityModel.Director)},
                logo = @{nameof(EntityModel.Logo)},
                is_deleted = false
            WHERE id = @{nameof(EntityModel.Id)}";

			await dbConnection.ExecuteAsync(query, updateValues);

			return Task.CompletedTask;
		}, _databaseService);
	}

	public async Task UpdateEntity(EntityTableModel updateValues)
	{
		await RepositoryHelper.ExecuteWithErrorHandlingAsync(async dbConnection =>
		{
			var query = $@"
            UPDATE public.entity
            SET 
                ""type"" = @{nameof(EntityTableModel.Type)},
                type_id = @{nameof(EntityTableModel.Type_ID)}
            WHERE id = @{nameof(EntityTableModel.Id)}";

			await dbConnection.ExecuteAsync(query, updateValues);

			return Task.CompletedTask;
		}, _databaseService);
	}
	//public async Task UpdateEntityManagers(EntityManagerModel updateValues)
	//{
	//	await RepositoryHelper.ExecuteWithErrorHandlingAsync(async dbConnection =>
	//	{
	//		var query = $@"
	//           UPDATE public.entity_managers
	//           SET 
	//               entity_id = @{nameof(EntityManagerModel.)},
	//               user_id = @{nameof(EntityManagerModel.UserId)},
	//               ""access"" = @{nameof(EntityManagerModel.Access)}
	//           WHERE id = @{nameof(EntityManagerModel.Id)}";

	//		await dbConnection.ExecuteAsync(query, updateValues);
	//	}, _databaseService);
	//}

	public async Task UpdateEntityStorage(EntityStorageModel updateValues)
	{
		await RepositoryHelper.ExecuteWithErrorHandlingAsync(async dbConnection =>
		{
			var query = $@"
            UPDATE public.entity_storage
            SET 
                id = @{nameof(EntityStorageModel.Id)},
                entity_id = @{nameof(EntityStorageModel.Entity_ID)},
                country = @{nameof(EntityStorageModel.Country)},
                city = @{nameof(EntityStorageModel.City)},
                address = @{nameof(EntityStorageModel.Address)},
                ""index"" = @{nameof(EntityStorageModel.Index)},
                is_deleted = false
            WHERE point = @{nameof(EntityStorageModel.Point)}";

			await dbConnection.ExecuteAsync(query, updateValues);
			return Task.CompletedTask;

		}, _databaseService);
	}



	public async Task DeleteData<T>(TableNames tableName, Guid id)
	{
		await RepositoryHelper.ExecuteWithErrorHandlingAsync(async dbConnection =>
		{
			var query = $@"delete 
				from ""{tableName.GetDescription()}""
				where id = @Id";

			await dbConnection.ExecuteAsync(query, new { Id = id });

			return Task.CompletedTask;

		}, _databaseService);
	}
	public async Task UpdateOrder(OrderModel updateValues)
	{
		await RepositoryHelper.ExecuteWithErrorHandlingAsync(async dbConnection =>
		{
			var query = $@"
            UPDATE public.""order""
            SET 
                entity_id = @{nameof(OrderModel.Entity_ID)},
                entity_managers_id = @{nameof(OrderModel.Entity_Managers_ID)},
                inn = @{nameof(OrderModel.INN)},
                kpp = @{nameof(OrderModel.KPP)},
                ogrn = @{nameof(OrderModel.OGRN)},
                fullname = @{nameof(OrderModel.FullName)},
                address = @{nameof(OrderModel.Address)},
                payment_account = @{nameof(OrderModel.Payment_Account)},
                tocor_account = @{nameof(OrderModel.ToCor_Account)},
                tobik = @{nameof(OrderModel.ToBIK)},
                tobank = @{nameof(OrderModel.ToBank)},
                fromcor_account = @{nameof(OrderModel.FromCor_Account)},
                frombik = @{nameof(OrderModel.FromBIK)},
                frombank = @{nameof(OrderModel.FromBank)},
                plan_date_shipment = @{nameof(OrderModel.Plan_Date_Shipment)},
                shipping_address = @{nameof(OrderModel.Shipping_Address)},
                application_date = @{nameof(OrderModel.Application_Date)},
                delivery_point = @{nameof(OrderModel.Delivery_Point)},
                delivery_address = @{nameof(OrderModel.Delivery_Address)},
                plan_date_receipt = @{nameof(OrderModel.Delivery_Address)},
                transporterfullname = @{nameof(OrderModel.Delivery_Address)},
                transportershortname = @{nameof(OrderModel.Delivery_Address)},
                ""comment"" = @{nameof(OrderModel.Comment)},
                vat = @{nameof(OrderModel.VAT)},
                is_deleted = false
            WHERE id = @{nameof(OrderModel.ID)}";

			await dbConnection.ExecuteAsync(query, updateValues);

			return Task.CompletedTask;

		}, _databaseService);
	}
	public async Task UpdateProduct(ProductModel updateValues)
	{
		await RepositoryHelper.ExecuteWithErrorHandlingAsync(async dbConnection =>
		{
			var query = $@"
            UPDATE public.product
            SET 
                entity_id = @{nameof(ProductModel.Entity_Id)},
                code = @{nameof(ProductModel.Code)},
                title = @{nameof(ProductModel.Title)},
                unit = @{nameof(ProductModel.Unit)},
                price = @{nameof(ProductModel.Price)},
                image = @{nameof(ProductModel.Image)},
                entity_storage_id = @{nameof(ProductModel.Entity_Storage_ID)},
                available_for_shipment = @{nameof(ProductModel.Available_For_Shipment)},
                party = @{nameof(ProductModel.Party)},
                implementation_period = @{nameof(ProductModel.Implementation_Period)},
                expiration_date = @{nameof(ProductModel.Expiration_Date)},
                is_deleted = false
            WHERE id = @{nameof(ProductModel.Id)}";

			await dbConnection.ExecuteAsync(query, updateValues);
			return Task.CompletedTask;

		}, _databaseService);
	}
	public async Task UpdateSupport(SupportModel updateValues)
	{
		await RepositoryHelper.ExecuteWithErrorHandlingAsync(async dbConnection =>
		{
			var query = $@"
            UPDATE public.support
            SET 
                email = @{nameof(SupportModel.Email)},
                entity_id = @{nameof(SupportModel.EntityId)},
                message = @{nameof(SupportModel.Message)},
                image = @{nameof(SupportModel.Image)}
            WHERE id = @{nameof(SupportModel.Id)}";

			await dbConnection.ExecuteAsync(query, updateValues);
			return Task.CompletedTask;

		}, _databaseService);
	}

	public async Task DeleteEntity(Guid id)
	{
		await RepositoryHelper.ExecuteWithErrorHandlingAsync(async dbConnection =>
		{
			var query = $@"delete 
				from ""{TableNames.Entity.GetDescription()}""
				where type_id = @Id";

			await dbConnection.ExecuteAsync(query, new { Id = id });

			return Task.CompletedTask;

		}, _databaseService);
	}
}
