using Storage.Domain.Models;

namespace Storage.Domain.Interfaces.Repositories;

public interface IEntityRepository
{
	public Task<EntityModel?> Get(Guid id, CancellationToken cancellationToken);
	public Task<EntityModel?> Get(string email, CancellationToken cancellationToken);

	public Task<Guid> Create(EntityModel entity, CancellationToken cancellationToken);
}
