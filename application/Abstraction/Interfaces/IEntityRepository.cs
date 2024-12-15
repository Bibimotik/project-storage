using application.MVVM.Model;

using CSharpFunctionalExtensions;

namespace application.Abstraction;

public interface IEntityRepository
{
	public Task<EntityModel?> Get(Guid id);
	public Task<EntityModel?> Get(string email);
	public Task<Guid> Create(EntityModel entity);
	Task<EntityTableModel?> GetEntity(Guid id);
}