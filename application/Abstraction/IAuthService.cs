namespace application.Abstraction;

public interface IAuthService
{
	public void SaveAuthData(string authEmail, string authPassword);
	public (string authEmail, string authPassword) LoadAuthData();
	public bool IsUserAuthenticated();
	public void ClearAuthData();
}