using System.Windows;

using application.Abstraction;
using application.Abstraction.Interfaces;
using application.MVVM.View;
using application.MVVM.View.Auth;
using application.MVVM.View.Pages;
using application.MVVM.ViewModel;
using application.MVVM.ViewModel.Auth;
using application.MVVM.ViewModel.Pages;
using application.Repository;
using application.Services;
using application.Services.Repository;
using application.Utilities;

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

		// Глобальная обработка исключений в потоке UI WPF
		this.DispatcherUnhandledException += App_DispatcherUnhandledException;

		// Обработка необработанных исключений в других потоках
		AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

		// Обработка необработанных исключений в задачах
		TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;

		Env.Load("../../../.env");

		IServiceCollection services = new ServiceCollection();

		services.AddScoped<IDatabaseService>(provider =>
			//new DatabaseService(Settings.Default.PostgresqlDev)
			new DatabaseService(Environment.GetEnvironmentVariable("POSTGRESQL"))
			);
		services.AddScoped<IEntityRepository, EntityRepository>();
		services.AddScoped<IAuthService, AuthService>();
		services.AddScoped<IMailService>(mail =>
			new MailService(
				"smtp.mail.ru",
				587,
				Environment.GetEnvironmentVariable("MAIL"),
				Environment.GetEnvironmentVariable("MAIL_PASSWORD"))
			);
		services.AddSingleton<INavigationService, NavigationService>();
		services.AddSingleton<ISecurityService, SecurityService>();
		services.AddTransient<RegistrationUserViewModel>();
		services.AddTransient<IParserINNService, ParserINNService>();
		//services.AddHttpClient<IEntityApi, EntityApi>(client =>
		//{
		//	client.BaseAddress = new Uri("http://localhost:5210/");
		//});
		services.AddScoped<IEntityService, EntityService>();

		services.AddSingleton<App>();

		services.AddTransient<AuthViewModel>();
		services.AddTransient<AuthView>();
		services.AddTransient<MainViewModel>();
		services.AddTransient<MainWindow>();
		services.AddTransient<AdminViewModel>();
		services.AddTransient<AdminView>();

		services.AddScoped<RegistrationCompanyStage1ViewModel>();
		services.AddScoped<RegistrationCompanyStage1View>();

		services.AddScoped<AccountViewModel>();
		services.AddScoped<AccountView>();
		services.AddScoped<StatisticsView>();
		services.AddScoped<SalesView>();
		services.AddScoped<StorageView>();
		services.AddScoped<StaffView>();
		services.AddScoped<SupportView>();
		services.AddScoped<InfoView>();

		services.AddScoped<IPasswordHash, PasswordHash>();

		_serviceProvider = services.BuildServiceProvider();

		IAuthService authService = _serviceProvider.GetRequiredService<IAuthService>();
		INavigationService navigationService = _serviceProvider.GetRequiredService<INavigationService>();
		ISecurityService securityService = _serviceProvider.GetRequiredService<ISecurityService>();
		securityService.GenerateKeys();

		//authService.ClearAuthData();

		switch (authService.IsUserAuthenticated())
		{
			case true:
				var (email, password) = authService.LoadAuthData();

				if (email == "admin" && password == "admin")
					navigationService.ShowAdmin();
				else
					navigationService.ShowMain();
				break;
			case false:
				navigationService.ShowAuth();
				break;
		}
	}

	private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
	{
		// Показать сообщение об ошибке
		MessageBox.Show($"Произошла ошибка: {e.Exception.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

		// Указать, что исключение обработано
		e.Handled = true;
	}

	private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
	{
		if (e.ExceptionObject is Exception ex)
		{
			MessageBox.Show($"Непредвиденная ошибка: {ex.Message}", "Критическая ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
		}
	}

	private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
	{
		// Показать сообщение об ошибке
		MessageBox.Show($"Ошибка в задаче: {e.Exception.Message}", "Ошибка задачи", MessageBoxButton.OK, MessageBoxImage.Error);

		// Указать, что исключение обработано
		e.SetObserved();
	}
}
