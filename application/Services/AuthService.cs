using System.Diagnostics;

using application.Abstraction;
using application.MVVM.Model;
using application.Properties;

namespace application.Services;

public class AuthService : IAuthService
{
	private readonly IEntityRepository _entityRepository;
	public AuthService(IEntityRepository entityRepository)
	{
		_entityRepository = entityRepository;
	}

	public async Task SaveAuthData(string authEmail, string authPassword)
	{
		if (authEmail != "admin")
			await GetUserData(authEmail);

		Settings.Default.AuthEmail = authEmail;
		Settings.Default.AuthPassword = authPassword;
		Settings.Default.Save();
	}

	public async Task<(string authEmail, string authPassword)> LoadAuthData()
	{
		var authEmail = Settings.Default.AuthEmail;
		var authPassword = Settings.Default.AuthPassword;

		if (authEmail != "admin")
			await GetUserData(authEmail);

		Debug.WriteLine("save data: " + authEmail + " " + authPassword);

		return (authEmail, authPassword);
	}

	public bool IsUserAuthenticated()
	{
		Debug.WriteLine("--------- " + Settings.Default.AuthEmail + " - " + Settings.Default.AuthPassword);
		return !string.IsNullOrEmpty(Settings.Default.AuthEmail);
	}

	public void ClearAuthData()
	{
		Settings.Default.AuthEmail = string.Empty;
		Settings.Default.AuthPassword = string.Empty;
		Settings.Default.Save();

		EntityModel.ResetOurUser();
	}

	private async Task GetUserData(string authEmail)
	{
		var model = await _entityRepository.Get(authEmail!);
		EntityModel.OurUserModel = model!;

		var entity = await _entityRepository.GetEntity(model!.Id);
		EntityModel.OurUserModel.EntityId = entity!.Id;
	}
}