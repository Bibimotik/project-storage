using System.Diagnostics;
using System.Net.Http;
using System.Windows;

using application.Abstraction;
using application.MVVM.Model;

using Newtonsoft.Json;

namespace application.Services;

public class ParserINNService : IParserINNService
{
	//TODO - если больше не будет в параматерах получения других пунктов, то совместить два метода в один
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

	// TODO -  Заменить на Result<ParserData>
	public async Task<(ParserModel?, string)> GetParserDataINN(string inputINN)
	{
		try
		{
			var parserData = await GetParserDataAsync(inputINN);

			if (parserData == null)
				return (null, "Не удалось получить данные.");
			//throw new InvalidOperationException("Не удалось получить данные.");


			EntityModel.Model ??= new EntityModel();
			EntityModel model = EntityModel.Model;

			model.INN = inputINN;
			model.KPP = parserData!.Kpp;
			model.FullName = parserData.FullName;
			model.ShortName = parserData.ShortName;
			model.OGRN = parserData.Ogrn;

			if (parserData.Director != null)
			{
				string cleanedDirector = parserData.Director
					.Replace("ГЕНЕРАЛЬНЫЙ", "")
					.Replace("ДИРЕКТОР", "")
					.Replace(":", "")
					.Trim();

				model.Director = cleanedDirector;
			}

			return (parserData!, string.Empty);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
			throw;
		}
	}
}