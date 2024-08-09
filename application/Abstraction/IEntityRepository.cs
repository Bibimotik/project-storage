using application.MVVM.Model;

using CSharpFunctionalExtensions;

namespace application.Abstraction;

public interface IEntityRepository
{
	public Task<EntityModel> GetEntityLogin(string email);
	public Task<Result<Guid>> UserRegistration(EntityModel entity);
	public Task<Result<Guid>> CompanyRegistration(EntityModel entity);
}