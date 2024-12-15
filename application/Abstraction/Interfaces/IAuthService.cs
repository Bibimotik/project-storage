namespace application.Abstraction;

public interface IAuthService
{
	public Task SaveAuthData(string authEmail, string authPassword);
	public Task<(string authEmail, string authPassword)> LoadAuthData();
	public bool IsUserAuthenticated();
	public void ClearAuthData();
}