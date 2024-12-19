using application.Abstraction;
using application.Abstraction.Interfaces;
using application.MVVM.Model;

using CSharpFunctionalExtensions;

namespace application.Services.Repository;

public class EntityService : IEntityService
{
	private readonly IEntityRepository _entitiesRepository;
	private readonly IPasswordHash _passwordHash;

	public EntityService(IEntityRepository entitiesRepository, IPasswordHash passwordHash)
	{
		_entitiesRepository = entitiesRepository;
		_passwordHash = passwordHash;
	}

	public async Task<Result> Login(string email, string password)
	{
		var existUser = await _entitiesRepository.Get(email);

		if (existUser is null)
			return Result.Failure("email");

		var isCorrectPassword = _passwordHash.Verify(password, existUser.Password);

		if (!isCorrectPassword)
			return Result.Failure("password");

		return Result.Success();
	}

	public async Task<Result<EntityModel>> Registration(EntityModel model)
	{
		var existUser = await _entitiesRepository.Get(model.Email);

		if (existUser is not null)
			return Result.Failure<EntityModel>($"Пользователь с почтой {model.Email} уже существует");

		model.Id = Guid.NewGuid();
		model.Password = _passwordHash.Generate(model.Password);

		await _entitiesRepository.Create(model);

		return model;
	}
}