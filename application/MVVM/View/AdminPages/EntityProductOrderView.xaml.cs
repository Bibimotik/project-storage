using System.Diagnostics;
using System.Windows.Controls;

using application.MVVM.Model;
using application.MVVM.ViewModel.AdminPages;

namespace application.MVVM.View.AdminPages;

/// <summary>
/// Логика взаимодействия для ProductOrderView.xaml
/// </summary>
public partial class EntityProductOrderView : UserControl
{
	public EntityProductOrderView(EntityProductOrderViewModel vm)
	{
		DataContext = vm;
		InitializeComponent();
	}

	private void DataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
	{
		// Получаем редактируемую строку
		if (e.Row.Item is EntityProductOrderModel editedRow)
		{
			// Уведомляем ViewModel об изменении строки
			var viewModel = DataContext as EntityProductOrderViewModel;
			viewModel?.MarkAsModified(editedRow);
			Debug.WriteLine("456");
		}
	}
}
