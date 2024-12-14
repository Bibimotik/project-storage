using System.Diagnostics;

using application.Abstraction;
using application.MVVM.Model;
using application.Utilities;

using Dapper;

using static application.Abstraction.EntityAbstraction;

namespace application.Repository;

public class EntityRepository : IEntityRepository
{
	private readonly IDatabaseService _databaseService;

	public EntityRepository(IDatabaseService databaseService) => _databaseService = databaseService;

	public async Task<EntityModel?> Get(Guid id)
	{
		return await RepositoryHelper.ExecuteWithErrorHandlingAsync(async dbConnection =>
		{
			string userQuery = $@"SELECT 
                id as {nameof(EntityModel.Id)},
                firstname as {nameof(EntityModel.FirstName)},
                secondname as {nameof(EntityModel.SecondName)},
                thirdname as {nameof(EntityModel.ThirdName)},
                phone as {nameof(EntityModel.Phone)},
                email as {nameof(EntityModel.Email)}, 
                password as {nameof(EntityModel.Password)}, 
                logo as {nameof(EntityModel.Logo)}
                FROM ""user"" 
                WHERE user_id = @{nameof(EntityModel.Id)}";

			var user = await dbConnection.QuerySingleOrDefaultAsync<EntityModel>(new CommandDefinition(userQuery, new { Id = id }));

			if (user != null)
				return user;

			string companyQuery = $@"SELECT 
                id as {nameof(EntityModel.Id)}, 
                inn as {nameof(EntityModel.INN)}, 
                kpp as {nameof(EntityModel.KPP)}, 
                ogrn as {nameof(EntityModel.OGRN)}, 
                fullname as {nameof(EntityModel.FullName)}, 
                shortname as {nameof(EntityModel.ShortName)}, 
                email as {nameof(EntityModel.Email)}, 
                password as {nameof(EntityModel.Password)}, 
                legal_address as {nameof(EntityModel.LegalAddress)}, 
                postal_address as {nameof(EntityModel.PostalAddress)}, 
                director as {nameof(EntityModel.Director)}, 
                logo as {nameof(EntityModel.Logo)}
                FROM company 
                WHERE company_id = @{nameof(EntityModel.Id)}";

			var company = await dbConnection.QuerySingleOrDefaultAsync<EntityModel>(new CommandDefinition(companyQuery, new { Id = id }));

			if (company != null)
				return company;

			return null;

		}, _databaseService);
	}

	public async Task<EntityModel?> Get(string email)
	{
		return await RepositoryHelper.ExecuteWithErrorHandlingAsync(async dbConnection =>
		{
			string userQuery = $@"SELECT 
                id as {nameof(EntityModel.Id)},
                firstname as {nameof(EntityModel.FirstName)},
                secondname as {nameof(EntityModel.SecondName)},
                thirdname as {nameof(EntityModel.ThirdName)},
                phone as {nameof(EntityModel.Phone)},
                email as {nameof(EntityModel.Email)}, 
                password as {nameof(EntityModel.Password)}, 
                logo as {nameof(EntityModel.Logo)}
                FROM ""user"" 
                WHERE email = @{nameof(EntityModel.Email)}";

			var user = await dbConnection.QuerySingleOrDefaultAsync<EntityModel>(new CommandDefinition(userQuery, new { Email = email }));

			if (user != null)
				return user;

			string companyQuery = $@"SELECT 
                id as {nameof(EntityModel.Id)},
                inn as {nameof(EntityModel.INN)}, 
                kpp as {nameof(EntityModel.KPP)}, 
                ogrn as {nameof(EntityModel.OGRN)}, 
                fullname as {nameof(EntityModel.FullName)}, 
                shortname as {nameof(EntityModel.ShortName)}, 
                email as {nameof(EntityModel.Email)}, 
                password as {nameof(EntityModel.Password)}, 
                legal_address as {nameof(EntityModel.LegalAddress)}, 
                postal_address as {nameof(EntityModel.PostalAddress)}, 
                director as {nameof(EntityModel.Director)}, 
                logo as {nameof(EntityModel.Logo)}
                FROM company 
                WHERE email = @{nameof(EntityModel.Email)}";

			var company = await dbConnection.QuerySingleOrDefaultAsync<EntityModel>(new CommandDefinition(companyQuery, new { Email = email }));

			if (company != null)
				return company;

			return null;

		}, _databaseService);
	}

	public async Task<Guid> Create(EntityModel entityModel)
	{
		return await RepositoryHelper.ExecuteWithErrorHandlingAsync(async dbConnection =>
		{
			string query = string.Empty;

			if (entityModel.EntityType == EntityType.User)
				query = $@"INSERT into ""user"" 
                    (id, firstname, secondname, thirdname, phone, email, password, logo, is_deleted)
                    values (
                    @{nameof(EntityModel.Id)},
                    @{nameof(EntityModel.FirstName)},
                    @{nameof(EntityModel.SecondName)},
                    @{nameof(EntityModel.ThirdName)},
                    @{nameof(EntityModel.Phone)},
                    @{nameof(EntityModel.Email)},
                    @{nameof(EntityModel.Password)},
                    NULL,
                    FALSE)
                    returning id";
			else if (entityModel.EntityType == EntityType.Company)
				query = $@"INSERT into company
                    (id, inn, kpp, ogrn, fullname, shortname, email, password, legal_address, postal_address, director, logo, is_deleted)
                    values (
                    @{nameof(EntityModel.Id)},
                    @{nameof(EntityModel.INN)},
                    @{nameof(EntityModel.KPP)},
                    @{nameof(EntityModel.OGRN)},
                    @{nameof(EntityModel.FullName)},
                    @{nameof(EntityModel.ShortName)},
                    @{nameof(EntityModel.Email)},
                    @{nameof(EntityModel.Password)},
                    @{nameof(EntityModel.LegalAddress)},
                    @{nameof(EntityModel.PostalAddress)},
                    @{nameof(EntityModel.Director)},
                    NULL,
                    FALSE)
                    returning id";

			EntityTableModel entity = new(
				Guid.NewGuid(),
				entityModel.EntityType == EntityType.User ? EntityType.User.GetDescription() : EntityType.Company.GetDescription(),
				entityModel.Id);

			string queryEntity = $@"INSERT INTO entity 
				(id, type, type_id) 
				values (
				@{nameof(EntityTableModel.Id)}, 
				@{nameof(EntityTableModel.Type)}, 
				@{nameof(EntityTableModel.Type_ID)}
				)";

			Debug.WriteLine(query);
			Debug.WriteLine(queryEntity);

			using var transaction = dbConnection.BeginTransaction();
			try
			{
				var insertedId = await dbConnection.QuerySingleAsync<Guid>(new CommandDefinition(query, entityModel));

				await dbConnection.ExecuteAsync(new CommandDefinition(queryEntity, entity));

				transaction.Commit();

				return insertedId;
			}
			catch
			{
				transaction.Rollback();
				throw;
			}
		}, _databaseService);
	}
}
