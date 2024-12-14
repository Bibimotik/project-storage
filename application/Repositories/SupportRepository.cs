using application.Abstraction;
using application.Abstraction.Interfaces;
using application.MVVM.Model;
using application.Utilities;

using Dapper;

namespace application.Repository;

public class SupportRepository : ISupportRepository
{
	private readonly IDatabaseService _databaseService;
	
	public SupportRepository(IDatabaseService databaseService) => _databaseService = databaseService;

	public async Task<Guid> SentToSupport(SupportModel supportModel)
	{
		return await RepositoryHelper.ExecuteWithErrorHandlingAsync(async dbConnection =>
		{
			string query = $@"INSERT INTO SUPPORT
                        (ID, Entity_ID, Email, Message, Image)
                        VALUES (
                         @{nameof(SupportModel.Id)},
                         @{nameof(SupportModel.Entity_ID)},
                         @{nameof(SupportModel.Email)},
                         @{nameof(SupportModel.Message)},
                         @{nameof(SupportModel.Image)}
                         )
                        RETURNING ID";

			Guid supportId = await dbConnection.QuerySingleAsync<Guid>(query, new
			{
				supportModel.Id,
				supportModel.Entity_ID,
				supportModel.Email,
				supportModel.Message,
				supportModel.Image
			});

			return supportId;
		}, _databaseService);
	}
}