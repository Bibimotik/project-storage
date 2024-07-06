using System.Windows;
using application.MVVM.View;

namespace application
{
	/// <summary>
	/// Логика взаимодействия для App.xaml
	/// </summary>
	public partial class App : Application
	{

		private Window _currentWindow;
		
		
		protected override void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);

			ShowAuth();
			// ShowMain();
		}
		
		private void ShowMain()
		{
			MainWindow mainWindow = new MainWindow();
			mainWindow.Show();

			CloseCurrentWindow();

			_currentWindow = mainWindow;
		}

		private void ShowAuth()
		{
			AuthView authWindow = new AuthView();
			authWindow.Show();

			CloseCurrentWindow();

			_currentWindow = authWindow;
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
}
