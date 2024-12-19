using System.Diagnostics;
using System.Windows.Controls;

using application.MVVM.Model;
using application.MVVM.ViewModel.AdminPages;

namespace application.MVVM.View.AdminPages;

/// <summary>
/// Логика взаимодействия для EntityView.xaml
/// </summary>
public partial class EntityView : UserControl
{
	public EntityView(EntityViewModel vm)
	{
		DataContext = vm;
		InitializeComponent();
	}

	private void DataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
	{
		// Получаем редактируемую строку
		if (e.Row.Item is EntityTableModel editedRow)
		{
			// Уведомляем ViewModel об изменении строки
			var viewModel = DataContext as EntityViewModel;
			viewModel?.MarkAsModified(editedRow);
		}
	}
}
