using System.Windows.Controls;

using application.MVVM.Model;
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
}
