using application.Abstraction;
using application.Abstraction.Interfaces;
using application.MVVM.Model;

using CSharpFunctionalExtensions;

namespace application.Services.Repository;

public class EntityStorageService : IEntityStorageService
{
	private readonly IEntityStorageRepository _entitiesRepository;

	public EntityStorageService(IEntityStorageRepository entitiesRepository)
	{
		_entitiesRepository = entitiesRepository;
	}

	public async Task<Guid> Insert(Guid id, EntityStorageModel model)
	{
		return await _entitiesRepository.InsertEntityStorage(id, model);
	}
}