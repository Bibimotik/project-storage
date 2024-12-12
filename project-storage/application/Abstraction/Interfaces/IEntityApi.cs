using application.MVVM.Model;

using CSharpFunctionalExtensions;

namespace application.Abstraction;

public interface IEntityApi
{
	public Task<Result> Login(string email, string password);
	public Task<Result<Guid>> UserRegistration(EntityModel model);
	public Task<Result> IsUserExist(string email);
}
