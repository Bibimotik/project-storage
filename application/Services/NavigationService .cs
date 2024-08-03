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

	// TODO - исправить ошибку 
	// System.InvalidOperationException: "Нельзя задать Visibility или вызвать Show, ShowDialog или WindowInteropHelper.EnsureHandle после закрытия окна."
	// возникает через раз
	// P.S у меня в курсаче было норм а щас хуй знает
	public void ShowAuth()
	{
		AuthView authWindow = _serviceProvider.GetRequiredService<AuthView>();
		authWindow.Show();

		CloseCurrentWindow();
		_currentWindow = authWindow;
	}

	public void ShowMain()
	{
		MainWindow mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
		mainWindow.Show();

		CloseCurrentWindow();
		_currentWindow = mainWindow;
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
