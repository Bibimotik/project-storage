using application.MVVM.Model;

namespace application.Abstraction;

public interface IUserRepository
{
	public LoginModel GetUserLogin(string email);
}