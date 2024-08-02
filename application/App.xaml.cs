using System.Configuration;
using System.Windows;

using application.Abstraction;
using application.MVVM.View;
using application.MVVM.ViewModel;
using application.Repository;
using application.Services;

using Microsoft.Extensions.DependencyInjection;

namespace application;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
	private IServiceProvider? _serviceProvider;

	protected override void OnStartup(StartupEventArgs e)
	{
		base.OnStartup(e);

		IServiceCollection services = new ServiceCollection();

		//services.AddMemoryCache();

		services.AddScoped<IDatabaseService>(provider => new DatabaseService(
			ConfigurationManager.ConnectionStrings["postgresql"].ConnectionString
			));
		services.AddScoped<IStatusRepository, StatusRepository>();
		services.AddScoped<IAuthService, AuthService>();

		services.AddSingleton<AuthViewModel>();
		services.AddSingleton<AuthView>();
		services.AddSingleton<MainWindow>();

		_serviceProvider = services.BuildServiceProvider();

		// TODO - механизм проверки сохраненной авторизации
		ShowAuth(_serviceProvider);
		//ShowMain(_serviceProvider);
	}

	private void ShowAuth(IServiceProvider serviceProvider)
	{
		AuthView authWindow = serviceProvider.GetRequiredService<AuthView>();
		authWindow.Show();
	}

	private void ShowMain(IServiceProvider serviceProvider)
	{
		MainWindow mainWindow = serviceProvider.GetRequiredService<MainWindow>();
		mainWindow.Show();
	}
}