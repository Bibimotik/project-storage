using application.MVVM.Model;

using CSharpFunctionalExtensions;

namespace application.Abstraction;

public interface IEntityRepository
{
	public Task<EntityModel?> Get(Guid id);
	public Task<EntityModel?> Get(string email);
	public Task<Guid> Create(EntityModel entity);
	//public Task<EntityModel> GetEntityLogin(string email);
	//public Task<Result<Guid>> UserRegistration(EntityModel entity);
	//public Task<Result<Guid>> CompanyRegistration(EntityModel entity);
	//public Result IsEmailExist(string email);
	public Task SendToSupport(EntityModel entity);
}