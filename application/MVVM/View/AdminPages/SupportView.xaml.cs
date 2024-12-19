using System.Diagnostics;
using System.Windows.Controls;

using application.MVVM.Model;
using application.MVVM.ViewModel.AdminPages;

namespace application.MVVM.View.AdminPages;

/// <summary>
/// Логика взаимодействия для SupportView.xaml
/// </summary>
public partial class SupportView : UserControl
{
	public SupportView(SupportViewModel vm)
	{
		DataContext = vm;
		InitializeComponent();
	}

	private void DataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
	{
		// Получаем редактируемую строку
		if (e.Row.Item is SupportModel editedRow)
		{
			// Уведомляем ViewModel об изменении строки
			var viewModel = DataContext as SupportViewModel;
			viewModel?.MarkAsModified(editedRow);
		}
	}
}
