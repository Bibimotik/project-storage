
using System.Diagnostics;

using Newtonsoft.Json;

using StackExchange.Redis;

namespace application.Services
{
	class RedisCacheService
	{
		private readonly ConnectionMultiplexer _redis;
		private readonly IDatabase _db;
		private readonly string _key = "AuthCredentials"; // Константный ключ

		public RedisCacheService()
		{
			var options = new ConfigurationOptions
			{
				EndPoints = { "redis.railway.internal:6379" },
				Password = "dDuxYRojVaWKXtHblgHEdqLqSthQGzIm",
				AbortOnConnectFail = false,
				ConnectRetry = 5, // Количество повторных попыток подключения
                ConnectTimeout = 5000, // Таймаут подключения в миллисекундах
                ReconnectRetryPolicy = new ExponentialRetry(5000) // Политика повторных попыток подключения
            };

			try
			{
				_redis = ConnectionMultiplexer.Connect(options);
				_db = _redis.GetDatabase();
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"Ошибка подключения к Redis: {ex.Message}");
				throw;
			}
		}

		public void SetUserCredentials(UserCredentials credentials)
		{
			var json = JsonConvert.SerializeObject(credentials);
			_db.StringSet(_key, json);
		}

		public UserCredentials? GetUserCredentials()
		{
			var json = _db.StringGet(_key);
			return json.IsNullOrEmpty ? null : JsonConvert.DeserializeObject<UserCredentials>(json);
		}
	}

	class UserCredentials
	{
		public string? Username { get; set; }
		public string? Password { get; set; }
	}
}
