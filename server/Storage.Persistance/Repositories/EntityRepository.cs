using Storage.Domain.Interfaces.Repositories;
using Storage.Domain.Interfaces;
using Storage.Domain.Models;
using Dapper;
using Storage.Domain.Abstractions;
using Storage.Infrastructure;
using Storage.Application.Exceptions;

namespace Storage.Persistance.Repositories;

public class EntityRepository(IStorageDBContext context) : IEntityRepository
{
	private readonly IStorageDBContext _context = context;

	public async Task<EntityModel?> Get(Guid id, CancellationToken cancellationToken)
	{
		return await RepositoryHelper.ExecuteWithErrorHandling(async dbConnection =>
		{
			string userQuery = $@"SELECT 
                firstname as {nameof(EntityModel.FirstName)},
                secondname as {nameof(EntityModel.SecondName)},
                thirdname as {nameof(EntityModel.ThirdName)},
                phone as {nameof(EntityModel.Phone)},
                email as {nameof(EntityModel.Email)}, 
                password as {nameof(EntityModel.Password)}, 
                logo as {nameof(EntityModel.Logo)}
                FROM ""user"" 
                WHERE user_id = @{nameof(EntityModel.Id)}";

			var user = await dbConnection.QuerySingleOrDefaultAsync<EntityModel>(new CommandDefinition(userQuery, new { Id = id }, cancellationToken: cancellationToken));

			if (user != null)
				return user;

			string companyQuery = $@"SELECT 
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

			var company = await dbConnection.QuerySingleOrDefaultAsync<EntityModel>(new CommandDefinition(companyQuery, new { Id = id }, cancellationToken: cancellationToken));

			if (company != null)
				return company;

			return null;

		}, _context, cancellationToken);
	}

	public async Task<EntityModel?> Get(string email, CancellationToken cancellationToken)
	{
		return await RepositoryHelper.ExecuteWithErrorHandling(async dbConnection =>
		{
			string userQuery = $@"SELECT 
                firstname as {nameof(EntityModel.FirstName)},
                secondname as {nameof(EntityModel.SecondName)},
                thirdname as {nameof(EntityModel.ThirdName)},
                phone as {nameof(EntityModel.Phone)},
                email as {nameof(EntityModel.Email)}, 
                password as {nameof(EntityModel.Password)}, 
                logo as {nameof(EntityModel.Logo)}
                FROM ""user"" 
                WHERE email = @{nameof(EntityModel.Email)}";

			var user = await dbConnection.QuerySingleOrDefaultAsync<EntityModel>(new CommandDefinition(userQuery, new { Email = email }, cancellationToken: cancellationToken));

			if (user != null)
				return user;

			string companyQuery = $@"SELECT 
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

			var company = await dbConnection.QuerySingleOrDefaultAsync<EntityModel>(new CommandDefinition(companyQuery, new { Email = email }, cancellationToken: cancellationToken));

			if (company != null)
				return company;

            return null;

		}, _context, cancellationToken);
	}

	public async Task<Guid> Create(EntityModel entity, CancellationToken cancellationToken)
	{
		return await RepositoryHelper.ExecuteWithErrorHandling(async dbConnection =>
		{
			string query = string.Empty;

			if (entity.Type == EntityType.User)
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
			else if (entity.Type == EntityType.Company)
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

			return await dbConnection.QuerySingleAsync<Guid>(new CommandDefinition(query, entity, cancellationToken: cancellationToken));
		}, _context, cancellationToken);
	}

	public async Task SendToSupport(EntityModel entity, CancellationToken cancellationToken)
	{
		await RepositoryHelper.ExecuteWithErrorHandling(async dbConnection =>
		{
			string insertSupportQuery = $@"INSERT INTO support
                                        (email, message)
                                        VALUES 
                                        (@Email, @Message) 
                                        RETURNING Support_ID";

			int supportId = await dbConnection.QuerySingleAsync<int>(new CommandDefinition(insertSupportQuery, new
			{
				entity.Email,
				entity.Message
			}, cancellationToken: cancellationToken));

			if (entity.Images != null)
			{
				string insertImageQuery = $@"INSERT INTO support_images
                                          (support_id, image)
                                          VALUES 
                                          (@SupportId, @Image)";

				await dbConnection.ExecuteAsync(new CommandDefinition(insertImageQuery, new
				{
					SupportId = supportId,
					Image = entity.Images
				}, cancellationToken: cancellationToken));
			}

			return Task.CompletedTask;
		}, _context, cancellationToken);
	}
}