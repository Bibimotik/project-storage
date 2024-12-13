using System.Diagnostics;

using application.Abstraction;
using application.MVVM.Model;
using application.Utilities;

using CSharpFunctionalExtensions;

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
                user_id as {nameof(EntityModel.Id)},
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
                company_id as {nameof(EntityModel.Id)}, 
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
                user_id as {nameof(EntityModel.Id)},
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
                company_id as {nameof(EntityModel.Id)},
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

	public async Task<Guid> Create(EntityModel entity)
	{
		return await RepositoryHelper.ExecuteWithErrorHandlingAsync(async dbConnection =>
		{
			string query = string.Empty;

			if (entity.EntityType == EntityType.User)
				query = $@"INSERT into ""user"" 
                    (user_id, firstname, secondname, thirdname, phone, email, password, logo, is_deleted)
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
                    returning user_id";
			else if (entity.EntityType == EntityType.Company)
				query = $@"INSERT into company
                    (company_id, inn, kpp, ogrn, fullname, shortname, email, password, legal_address, postal_address, director, logo, is_deleted)
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
                    returning company_id";

			//@{ (entity.EntityType == EntityType.User ? "User" : "Company")}, 


			string queryEntity = $@"INSERT INTO entity 
				(entity_id, type, type_id) 
				values (
				{Guid.NewGuid()}
				{(entity.EntityType == EntityType.User ? $@"'User'" : $@"'Company'")}, 
				@{nameof(EntityModel.Id)}
				)";

			Debug.WriteLine(queryEntity);

			using var transaction = dbConnection.BeginTransaction();
			try
			{
				var insertedId = await dbConnection.QuerySingleAsync<Guid>(new CommandDefinition(query, entity));

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

	public async Task SendToSupport(EntityModel entity)
	{
		await RepositoryHelper.ExecuteWithErrorHandlingAsync(async dbConnection =>
		{
			string insertSupportQuery = $@"INSERT INTO support
                                        (email, message)
                                        VALUES 
                                        (@Email, @Message) 
                                        RETURNING Support_ID";

			int supportId = await dbConnection.QuerySingleAsync<int>(insertSupportQuery, new
			{
				Email = entity.Email,
				Message = entity.Message
			});

			if (entity.Images != null)
			{
				string insertImageQuery = $@"INSERT INTO support_images
                                          (support_id, image)
                                          VALUES 
                                          (@SupportId, @Image)";

				await dbConnection.ExecuteAsync(insertImageQuery, new
				{
					SupportId = supportId,
					Image = entity.Images
				});
			}

			return Task.CompletedTask;
		}, _databaseService);
	}
}
