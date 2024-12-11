using System.Net.Http;
using System.Text;

using application.Abstraction;
using application.MVVM.Model;

using CSharpFunctionalExtensions;

using Newtonsoft.Json;

namespace application.API;
public class UserResponse
{
	public Guid Id { get; set; }
	public string FirstName { get; set; }
	public string SecondName { get; set; }
	public string ThirdName { get; set; }
	public string Phone { get; set; }
	public string Email { get; set; }
	public string Password { get; set; }
	public string Logo { get; set; } // Если логотип в Base64
	public string Type { get; set; }
}

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
		var wmodel = new 
		{
			firstName = model.FirstName,
			secondName = model.SecondName,
			thirdName = model.ThirdName,
			phone = model.Phone,
			email = model.Email,
			password = model.Password,
			confirmPassword = model.ConfirmPassword,
			entityType = model.EntityType.ToString(),
			logo = "" // Пустая строка или валидная Base64-строка
		};

		var jsonContent = new StringContent(JsonConvert.SerializeObject(wmodel), Encoding.UTF8, "application/json");
		var response = await _httpClient.PostAsync("User/UserRegistration", jsonContent);

		var rawResult = await response.Content.ReadAsStringAsync();
		Console.WriteLine($"Raw response: {rawResult}");

		if (!response.IsSuccessStatusCode)
			return Result.Failure<Guid>(rawResult);

		try
		{
			var userResponse = JsonConvert.DeserializeObject<UserResponse>(rawResult);
			return Result.Success(userResponse.Id);
		}
		catch (JsonException ex)
		{
			return Result.Failure<Guid>($"Failed to parse JSON: {ex.Message}\nRaw JSON: {rawResult}");
		}
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
