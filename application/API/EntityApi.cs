using System.Net.Http;
using System.Text;

using application.Abstraction;
using application.MVVM.Model;

using CSharpFunctionalExtensions;

using Newtonsoft.Json;

namespace application.API;

public class EntityApi() : IEntityApi
{
	private readonly HttpClient _httpClient = new()
	{
		BaseAddress = new Uri("http://localhost:5210/")
	};

	public async Task<Result> Login(string email, string password)
	{
		var loginRequest = new
		{
			Email = email,
			Password = password
		};

		var jsonContent = new StringContent(JsonConvert.SerializeObject(loginRequest), Encoding.UTF8, "application/json");

		var response = await _httpClient.PostAsync("User/Login", jsonContent);

		if (response.IsSuccessStatusCode)
			return Result.Success();
		else
			return Result.Failure(await response.Content.ReadAsStringAsync());
	}


	public async Task<Result<Guid>> UserRegistration(EntityModel model)
	{
		var jsonContent = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
		var response = await _httpClient.PostAsync("User/UserRegistration", jsonContent);

		var result = await response.Content.ReadAsStringAsync();

		if (!response.IsSuccessStatusCode)
			return Result.Failure<Guid>(result);

		return Result.Success(JsonConvert.DeserializeObject<Guid>(result));
	}

	public async Task<Result> IsUserExist(string email)
	{
		var response = await _httpClient.GetAsync($"User/IsUserExist/{email}");

		var result = await response.Content.ReadAsStringAsync();

		if (!response.IsSuccessStatusCode)
			return Result.Failure(result);

		return Result.Success(JsonConvert.DeserializeObject<Guid>(result));
	}
}
