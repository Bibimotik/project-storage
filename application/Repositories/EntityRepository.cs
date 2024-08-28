using System.Diagnostics;

using application.Abstraction;
using application.MVVM.Model;
using application.Utilities;

using CSharpFunctionalExtensions;

using Dapper;

namespace application.Repository;

public class EntityRepository : IEntityRepository
{
	private readonly IDatabaseService _databaseService;

	public EntityRepository(IDatabaseService databaseService) => _databaseService = databaseService;

	public async Task<EntityModel> GetEntityLogin(string email)
	{
		return await RepositoryHelper.ExecuteWithErrorHandlingAsync(async dbConnection =>
		{
			string query = $@"SELECT 
				email as {nameof(EntityModel.Email)}, 
				password as {nameof(EntityModel.Password)} 
				FROM ""user"" 
				WHERE email = @{nameof(EntityModel.Email)}
				UNION
				SELECT email as {nameof(EntityModel.Email)}, 
				password as {nameof(EntityModel.Password)} 
				FROM company 
				WHERE email = @{nameof(EntityModel.Email)}";

			return await dbConnection.QuerySingleAsync<EntityModel>(query, new { Email = email });
		}, _databaseService);
	}

	public async Task<Result<Guid>> UserRegistration(EntityModel entity)
	{
		return await RepositoryHelper.ExecuteWithErrorHandlingAsync(async dbConnection =>
		{
			Result email = IsEmailExist(entity.Email);
			if (email.IsFailure)
				return Result.Failure<Guid>(email.Error);

			string query = $@"INSERT into ""user"" 
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

			return Result.Success(await dbConnection.QuerySingleAsync<Guid>(query, entity));
		}, _databaseService);
	}

	public async Task<Result<Guid>> CompanyRegistration(EntityModel entity)
	{
		return await RepositoryHelper.ExecuteWithErrorHandlingAsync(async dbConnection =>
		{
			Result email = IsEmailExist(entity.Email);
			if (email.IsFailure)
				return Result.Failure<Guid>(email.Error);

			string query = $@"INSERT into company
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

			return Result.Success(await dbConnection.QuerySingleAsync<Guid>(query, entity));
		}, _databaseService);
	}
	
	public Result IsEmailExist(string email)
	{
		if (string.IsNullOrWhiteSpace(email))
			return Result.Failure("Email cannot be empty or whitespace.");

		return RepositoryHelper.ExecuteWithErrorHandling(dbConnection =>
		{
			Debug.WriteLine("email " + email);
			string query = @"SELECT COUNT(1) 
				FROM ""user"" 
				WHERE email = @Email
				UNION ALL
				SELECT COUNT(1) 
				FROM company
				WHERE email = @Email";

			int emailCount = dbConnection.QueryFirstOrDefault<int>(query, new { Email = email });

			if (emailCount == 0)
				return Result.Success();

			return Result.Failure("Entity with this email already exists.");
		}, _databaseService);
	}
	
	public async Task SendToSupport(EntityModel entity)
	{
		await RepositoryHelper.ExecuteWithErrorHandlingAsync(async dbConnection =>
		{
			string query = $@"INSERT into support
	            (email, message)
	            values (
	            @{nameof(EntityModel.Email)},
	            @{nameof(EntityModel.Message)})";
			
			await dbConnection.ExecuteAsync(query, entity);
			return Task.CompletedTask;
		}, _databaseService);
	}
}
