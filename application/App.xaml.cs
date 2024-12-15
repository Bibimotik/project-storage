using System.Windows;

using application.Abstraction;
using application.Abstraction.Interfaces;
using application.MVVM.Model;
using application.MVVM.View;
using application.MVVM.View.AdminPages;
using application.MVVM.View.Auth;
using application.MVVM.View.Pages;
using application.MVVM.ViewModel;
using application.MVVM.ViewModel.AdminPages;
using application.MVVM.ViewModel.Auth;
using application.MVVM.ViewModel.Pages;
using application.Repositories;
using application.Repository;
using application.Services;
using application.Services.Repository;
using application.Utilities;

using DotNetEnv;

using Microsoft.Extensions.DependencyInjection;

using EntityProductView = application.MVVM.View.AdminPages.EntityProductView;

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
			new DatabaseService(Environment.GetEnvironmentVariable("POSTGRESQL"))
			);
		services.AddTransient<IEntityRepository, EntityRepository>();
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
		services.AddScoped<IEntityService, EntityService>();
		services.AddTransient<IEntityStorageRepository, EntityStorageRepository>();
		services.AddTransient<ISupportRepository, SupportRepository>();
		services.AddTransient<IAddStaffRepository, AddStaffRepository>();
		services.AddTransient<IRolesRepository, RolesRepository>();
		services.AddTransient<IStorageRepository, StorageRepository>();
		services.AddTransient<IStorageProductsRepository, StorageProductsRepository>();
		services.AddTransient<IAddProductRepository, AddProductRepository>();
		services.AddTransient<TablesRepository>();

		services.AddSingleton<App>();

		services.AddTransient<AuthViewModel>();
		services.AddTransient<AuthView>();
		services.AddTransient<MainViewModel>();
		services.AddTransient<MainWindow>();
		services.AddTransient<AdminViewModel>();
		services.AddTransient<AdminView>();

		services.AddScoped<RegistrationCompanyStage1ViewModel>();
		services.AddScoped<RegistrationCompanyStage1View>();

		services.AddTransient<AddStorageViewModel>();
		services.AddScoped<AddStorageView>();
		services.AddTransient<StorageViewModel>();
		services.AddScoped<StorageView>();

		services.AddScoped<AccountViewModel>();
		services.AddScoped<AccountView>();
		
		services.AddTransient<StatisticsViewModel>();
		services.AddScoped<StatisticsView>();
		
		services.AddTransient<SalesViewModel>();
		services.AddScoped<SalesView>();
		
		services.AddTransient<StaffViewModel>();
		services.AddScoped<StaffView>();
		
		services.AddTransient<AddStaffViewModel>();
		services.AddScoped<AddStaffView>();
		
		services.AddTransient<RolesViewModel>();
		services.AddScoped<RolesView>();
		
		services.AddTransient<StorageProductsViewModel>();
		services.AddScoped<StorageProductsView>();
		
		services.AddTransient<AddProductViewModel>();
		services.AddScoped<AddProductView>();

		services.AddTransient<CompanyView>();
		services.AddTransient<CompanyViewModel>();
		services.AddTransient<EntityManagersView>();
		services.AddTransient<EntityProductView>();
		services.AddTransient<EntityProductOrderView>();
		services.AddTransient<EntityStorageView>();
		services.AddTransient<EntityView>();
		services.AddTransient<OrderView>();
		services.AddTransient<ProductView>();
		services.AddTransient<MVVM.View.AdminPages.SupportView>();
		services.AddTransient<UserView>();

		services.AddTransient<MVVM.ViewModel.Pages.SupportViewModel>();
		services.AddScoped<SupportMainView>();
		services.AddScoped<MVVM.View.Pages.SupportView>();

		services.AddScoped<InfoView>();
		services.AddScoped<InfoMainView>();

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

				if (email == "admin" && password == "Admin123")
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
		MessageBox.Show($"Произошла ошибка: {e.Exception.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

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
		MessageBox.Show($"Ошибка в задаче: {e.Exception.Message}", "Ошибка задачи", MessageBoxButton.OK, MessageBoxImage.Error);

		e.SetObserved();
	}
}
