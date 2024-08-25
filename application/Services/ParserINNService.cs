using System.Diagnostics;
using System.Net.Http;

using application.Abstraction;
using application.MVVM.Model;

using Newtonsoft.Json;

namespace application.Services;

public class ParserINNService : IParserINNService
{
	public async Task<ParserModel> GetParserDataAsync(string inn)
	{
		using var httpClient = new HttpClient();
		var startInfo = new ProcessStartInfo
		{
			FileName = "python",
			Arguments = $"..\\..\\..\\..\\parser\\main.py {inn}",
			RedirectStandardOutput = true,
			UseShellExecute = false,
			CreateNoWindow = true
		};

		using var process = Process.Start(startInfo);
		using var reader = process.StandardOutput;
		string output = await reader.ReadToEndAsync();
		process.WaitForExit();

		if (string.IsNullOrEmpty(output))
			return null;

		try
		{
			var data = JsonConvert.DeserializeObject<Dictionary<string, string>>(output);
			return new ParserModel()
			{
				Kpp = data.GetValueOrDefault("p"),
				FullName = data.GetValueOrDefault("n"),
				ShortName = data.GetValueOrDefault("c"),
				Ogrn = data.GetValueOrDefault("o"),
				Director = data.GetValueOrDefault("g")
			};
		}
		catch (JsonException)
		{
			Debug.WriteLine("Ошибка при десериализации данных.");
			return null;
		}
	}
}