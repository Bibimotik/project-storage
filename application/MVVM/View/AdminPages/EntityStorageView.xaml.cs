using System.Windows.Controls;

using application.MVVM.Model;
using application.MVVM.ViewModel.AdminPages;

namespace application.MVVM.View.AdminPages;

/// <summary>
/// Логика взаимодействия для EntityStorageView.xaml
/// </summary>
public partial class EntityStorageView : UserControl
{
	public EntityStorageView(EntityStorageViewModel vm)
	{
		DataContext = vm;
		InitializeComponent();
	}

	private void DataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
	{
		// Получаем редактируемую строку
		if (e.Row.Item is EntityStorageModel editedRow)
		{
			// Уведомляем ViewModel об изменении строки
			var viewModel = DataContext as EntityStorageViewModel;
			viewModel?.MarkAsModified(editedRow);
		}
	}
}