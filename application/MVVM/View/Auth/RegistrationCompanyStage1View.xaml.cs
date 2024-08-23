using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;
using System.Collections.Generic;

using application.MVVM.Model;

namespace application.MVVM.View.Auth
{
    /// <summary>
    /// Логика взаимодействия для RegistrationStage1.xaml
    /// </summary>
    public partial class RegistrationCompanyStage1View : UserControl
    {
        public RegistrationCompanyStage1View()
        {
            InitializeComponent();
        }

        public async Task<ParserModel> GetParserDataAsync(string inn)
        {
            using var httpClient = new HttpClient();
            var startInfo = new ProcessStartInfo
            {
                FileName = "python",
                Arguments = $"D:\\Учеба\\project-storage\\parser\\main.py {inn}",
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
		// TODO (СРОЧНО) - привести это в божеский вид
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            string inn = INN.Text;
            var parserData = await GetParserDataAsync(inn);

            if (parserData != null)
            {
                KPP.Text = parserData.Kpp;
                FullName.Text = parserData.FullName;
                ShortName.Text = parserData.ShortName;
                OGRN.Text = parserData.Ogrn;
                
                EntityModel.Model ??= new EntityModel();

                EntityModel model = EntityModel.Model;
                model.KPP = parserData.Kpp;
                model.FullName = parserData.FullName;
                model.ShortName = parserData.ShortName;
                model.OGRN = parserData.Ogrn;
                model.Director = parserData.Director;
            }
            else
            {
                MessageBox.Show("Не удалось получить данные.");
            }
        }
    }
}
