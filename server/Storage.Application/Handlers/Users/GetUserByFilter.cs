using MediatR;

using Storage.Domain.Interfaces.Repositories;
using Storage.Domain.Models;

namespace Storage.Application.Handlers.Users;

public class GetUserByFilterQuery(Guid? userId = null, string? email = null) : IRequest<EntityModel?>
{
	public Guid? UserId { get; private set; } = userId;
	public string? Email { get; private set; } = email;
}

public class GetUserByFilterQueryHandler(IEntityRepository entitiesRepository) : IRequestHandler<GetUserByFilterQuery, EntityModel?>
{
	private readonly IEntityRepository _entitiesRepository = entitiesRepository;

	public async Task<EntityModel?> Handle(GetUserByFilterQuery request, CancellationToken cancellationToken)
	{
		EntityModel? userModel = null;

		if (request.UserId.HasValue)
			userModel = await _entitiesRepository.Get(request.UserId.Value, cancellationToken);
		else if (!string.IsNullOrEmpty(request.Email))
			userModel = await _entitiesRepository.Get(request.Email, cancellationToken);

		return userModel;
	}
}