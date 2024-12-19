using System.Diagnostics;
using System.Windows.Controls;

using application.MVVM.Model;
using application.MVVM.ViewModel.AdminPages;

namespace application.MVVM.View.AdminPages;

/// <summary>
/// Логика взаимодействия для EntityManagersView.xaml
/// </summary>
public partial class EntityManagersView : UserControl
{
	public EntityManagersView(EntityManagersViewModel vm)
	{
		DataContext = vm;
		InitializeComponent();
	}

	private void DataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
	{
		// Получаем редактируемую строку
		if (e.Row.Item is EntityManagerModel editedRow)
		{
			// Уведомляем ViewModel об изменении строки
			var viewModel = DataContext as EntityManagersViewModel;
			viewModel?.MarkAsModified(editedRow);
			Debug.WriteLine("456");
		}
	}
}
