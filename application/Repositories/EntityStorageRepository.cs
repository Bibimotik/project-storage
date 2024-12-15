using application.Abstraction;
using application.Abstraction.Interfaces;
using application.MVVM.Model;
using application.Utilities;

using CSharpFunctionalExtensions;

using Dapper;

namespace application.Repository;

public class EntityStorageRepository : IEntityStorageRepository
{
	private readonly IDatabaseService _databaseService;

	public EntityStorageRepository(IDatabaseService databaseService) => _databaseService = databaseService;

	public async Task<Guid> InsertEntityStorage(Guid entityId, EntityStorageModel storageModel)
	{
		return await RepositoryHelper.ExecuteWithErrorHandlingAsync(async dbConnection =>
		{
			string query = $@"INSERT INTO ENTITY_STORAGE
                        (ID, Entity_ID, Point, Country, City, Address, Index, Is_Deleted)
                        VALUES (
                        @{nameof(EntityStorageModel.Id)},
                        @EntityId,
                        @{nameof(EntityStorageModel.Point)},
                        @{nameof(EntityStorageModel.Country)},
                        @{nameof(EntityStorageModel.City)},
                        @{nameof(EntityStorageModel.Address)},
                        @{nameof(EntityStorageModel.Index)},
                         FALSE)
                        RETURNING ID";

			Guid storageId = await dbConnection.QuerySingleAsync<Guid>(query, new
			{
				storageModel.Id,
				EntityId = entityId,
				storageModel.Point,
				storageModel.Country,
				storageModel.City,
				storageModel.Address,
				storageModel.Index
			});

			return storageId;
		}, _databaseService);
	}
}