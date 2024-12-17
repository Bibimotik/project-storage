using System.Windows;

using application.Abstraction;
using application.Abstraction.Interfaces;
using application.MVVM.Model;
using application.MVVM.View;
using application.MVVM.View.Role;

using Microsoft.Extensions.DependencyInjection;

using static application.Abstraction.EntityAbstraction;

namespace application.Services;

public class NavigationService : INavigationService
{
	private Window? _currentWindow;
	private readonly IServiceProvider _serviceProvider;
	private readonly IRolesRepository _rolesRepository;

	public NavigationService(IServiceProvider serviceProvider, IRolesRepository rolesRepository)
	{
		_serviceProvider = serviceProvider;
		_rolesRepository = rolesRepository;
	}

	public void ShowAuth()
	{
		var authWindow = _serviceProvider.GetRequiredService<AuthView>();
		authWindow.ContentRendered += NewWindowContentRendered;
		authWindow.Show();
		EntityModel.OurUserModel.Role = UserRole.NoRole;
	}

	public void ShowMain()
	{
		var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
		mainWindow.ContentRendered += NewWindowContentRendered;
		mainWindow.Show();
		EntityModel.OurUserModel.Role = UserRole.NoRole;
	}

	public void ShowAdmin()
	{
		var adminWindow = _serviceProvider.GetRequiredService<AdminView>();
		adminWindow.ContentRendered += NewWindowContentRendered;
		adminWindow.Show();
		EntityModel.OurUserModel.Role = UserRole.NoRole;
	}

	public void ShowManagerRole()
	{
		var adminWindow = _serviceProvider.GetRequiredService<ManagerWindowView>();
		adminWindow.ContentRendered += NewWindowContentRendered;
		adminWindow.Show();
		EntityModel.OurUserModel.Role = UserRole.Manager;
	}

	public async void ShowWorkerRole()
	{
		var workerWindow = _serviceProvider.GetRequiredService<WorkerWindowView>();
		workerWindow.ContentRendered += NewWindowContentRendered;
		workerWindow.Show();
		// TODO - company_id
		EntityModel.OurUserModel.Role = UserRole.Worker;
		//EntityModel.OurUserModel.EntityId = await _rolesRepository.GetCompanyId(EntityModel.OurUserModel.EntityId);
	}

	public void ShowAnalystRole()
	{
		var adminWindow = _serviceProvider.GetRequiredService<AdminView>();
		adminWindow.ContentRendered += NewWindowContentRendered;
		adminWindow.Show();
		EntityModel.OurUserModel.Role = UserRole.Analyst;
	}

	private void NewWindowContentRendered(object sender, EventArgs e)
	{
		if (sender is Window newWindow)
		{
			newWindow.ContentRendered -= NewWindowContentRendered;
			CloseCurrentWindow();
			_currentWindow = newWindow;
		}
	}

	private void CloseCurrentWindow()
	{
		if (_currentWindow != null)
		{
			_currentWindow.Close();
			_currentWindow = null;
		}
	}
}