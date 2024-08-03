using System.Windows;

using application.Abstraction;
using application.MVVM.View;
using application.MVVM.View.Pages;
using application.MVVM.ViewModel;
using application.MVVM.ViewModel.Pages;
using application.Properties;
using application.Repository;
using application.Services;

using Microsoft.Extensions.DependencyInjection;

namespace application;

public partial class App : Application
{
	private static IServiceProvider _serviceProvider;

	protected override void OnStartup(StartupEventArgs e)
	{
		base.OnStartup(e);

		IServiceCollection services = new ServiceCollection();

		services.AddScoped<IDatabaseService>(provider => new DatabaseService(Settings.Default.Postgresql));
		services.AddScoped<IStatusRepository, StatusRepository>();
		services.AddScoped<IAuthService, AuthService>();
		services.AddSingleton<INavigationService, NavigationService>();

		services.AddSingleton<App>();
		services.AddScoped<AuthViewModel>();
		services.AddScoped<AuthView>();
		services.AddScoped<MainWindow>();
		services.AddScoped<MainViewModel>();
		services.AddScoped<AccountViewModel>();
		services.AddScoped<AccountView>();
		//services.AddScoped<StatisticsView>();
		//services.AddScoped<SalesView>();
		//services.AddScoped<StorageView>();
		//services.AddScoped<StaffView>();
		//services.AddScoped<SupportView>();
		//services.AddScoped<InfoView>();

		_serviceProvider = services.BuildServiceProvider();

		IAuthService authService = _serviceProvider.GetRequiredService<IAuthService>();
		INavigationService navigationService = _serviceProvider.GetRequiredService<INavigationService>();

		switch (authService.IsUserAuthenticated())
		{
			case true:
				navigationService.ShowMain();
				break;
			case false:
				navigationService.ShowAuth();
				break;
		}
	}
}
