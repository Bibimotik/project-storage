using System.Diagnostics;
using System.Windows.Controls;

using application.MVVM.Model;
using application.MVVM.ViewModel;
using application.MVVM.ViewModel.AdminPages;
using application.Utilities;

namespace application.MVVM.View.AdminPages;

/// <summary>
/// Логика взаимодействия для UserView.xaml
/// </summary>
public partial class UserView : UserControl
{
	public UserView(UserViewModel vm)
	{
		DataContext = vm;
		InitializeComponent();

		GenerateColumns();
	}

	private void GenerateColumns()
	{
		var columns = TableHelper.GetColumnsForAttribute<EntityModel>(typeof(RequiredForUserTableAttribute));

		foreach (var column in columns)
			MyDataGrid.Columns.Add(column);
	}

	private void DataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
	{
		// Получаем редактируемую строку
		if (e.Row.Item is EntityModel editedRow)
		{
			// Уведомляем ViewModel об изменении строки
			var viewModel = DataContext as UserViewModel;
			viewModel?.MarkAsModified(editedRow);
			Debug.WriteLine("456");
		}
	}
}
