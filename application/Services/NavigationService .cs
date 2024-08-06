using System.Windows;

using application.Abstraction;
using application.MVVM.View;

using Microsoft.Extensions.DependencyInjection;

namespace application.Services;

public class NavigationService : INavigationService
{
	private Window? _currentWindow;
	private readonly IServiceProvider _serviceProvider;

	public NavigationService(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;

	public void ShowAuth()
	{
		var authWindow = _serviceProvider.GetRequiredService<AuthView>();
		authWindow.ContentRendered += NewWindowContentRendered;
		authWindow.Show();
	}

	public void ShowMain()
	{
		var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
		mainWindow.ContentRendered += NewWindowContentRendered;
		mainWindow.Show();
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