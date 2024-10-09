using System.Windows;

using application.Abstraction;
using application.MVVM.View;
using application.MVVM.View.Pages;
using application.MVVM.ViewModel;
using application.MVVM.ViewModel.Auth;
using application.MVVM.ViewModel.Pages;
using application.Repository;
using application.Services;

using DotNetEnv;

using MailServiceLibrary;

using Microsoft.Extensions.DependencyInjection;

namespace application;

public partial class App : Application
{
	private static IServiceProvider? _serviceProvider;

	protected override void OnStartup(StartupEventArgs e)
	{
		base.OnStartup(e);

		Env.Load("../../../.env");

		IServiceCollection services = new ServiceCollection();

		services.AddScoped<IEntityRepository, EntityRepository>();
		services.AddScoped<IAuthService, AuthService>();
		services.AddScoped<IMailService>(mail => 
			new MailService(
				"smtp.mail.ru", 
				587, 
				Environment.GetEnvironmentVariable("MAIL"), 
				Environment.GetEnvironmentVariable("MAIL_PASSWORD")));
		services.AddSingleton<INavigationService, NavigationService>();
		services.AddSingleton<ISecurityService, SecurityService>();
		services.AddTransient<RegistrationUserViewModel>();
		services.AddTransient<IParserINNService, ParserINNService>();

		services.AddSingleton<App>();
		services.AddScoped<AuthViewModel>();
		services.AddScoped<AuthView>();
		services.AddScoped<MainWindow>();
		services.AddScoped<MainViewModel>();
		services.AddScoped<AccountViewModel>();
		services.AddScoped<AccountView>();
		services.AddScoped<StatisticsView>();
		services.AddScoped<SalesView>();
		services.AddScoped<StorageView>();
		services.AddScoped<StaffView>();
		services.AddScoped<SupportView>();
		services.AddScoped<InfoView>();

		_serviceProvider = services.BuildServiceProvider();

		IAuthService authService = _serviceProvider.GetRequiredService<IAuthService>();
		INavigationService navigationService = _serviceProvider.GetRequiredService<INavigationService>();
		ISecurityService securityService = _serviceProvider.GetRequiredService<ISecurityService>();
		securityService.GenerateKeys();

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
