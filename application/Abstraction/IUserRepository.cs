using application.MVVM.Model;

namespace application.Abstraction;

public interface IUserRepository
{
	public Task<EntityModel> GetUserLogin(string email);
	public Task<Guid> UserRegistration(EntityModel entity);
}