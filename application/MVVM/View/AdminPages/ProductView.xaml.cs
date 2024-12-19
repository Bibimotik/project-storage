using System.Diagnostics;
using System.Windows.Controls;

using application.MVVM.Model;
using application.MVVM.ViewModel.AdminPages;

namespace application.MVVM.View.AdminPages;

/// <summary>
/// Логика взаимодействия для EntityProductView.xaml
/// </summary>
public partial class ProductView : UserControl
{
	public ProductView(ProductViewModel vm)
	{
		DataContext = vm;
		InitializeComponent();
	}

	private void DataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
	{
		// Получаем редактируемую строку
		if (e.Row.Item is ProductModel editedRow)
		{
			// Уведомляем ViewModel об изменении строки
			var viewModel = DataContext as ProductViewModel;
			viewModel?.MarkAsModified(editedRow);
		}
	}
}
