using System.Windows;

using application.MVVM.View;

namespace application;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
	protected override void OnStartup(StartupEventArgs e)
	{
		base.OnStartup(e);

		// TODO - механизм проверки сохраненной авторизации

		//ShowAuth();
		ShowMain();
	}

	private void ShowMain()
	{
		MainWindow mainWindow = new MainWindow();
		mainWindow.Show();

		//CloseCurrentWindow();

		//_currentWindow = mainWindow;

	}

	private void ShowAuth()
	{
		AuthView authWindow = new AuthView();
		authWindow.Show();

		//CloseCurrentWindow();

		//_currentWindow = authWindow;
	}
}