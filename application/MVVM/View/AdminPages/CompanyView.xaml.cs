using System.Windows.Controls;

using application.MVVM.Model;
using application.MVVM.ViewModel.AdminPages;
using application.Utilities;

namespace application.MVVM.View.AdminPages;

/// <summary>
/// Логика взаимодействия для CompanyView.xaml
/// </summary>
public partial class CompanyView : UserControl
{
	public CompanyView(CompanyViewModel company)
	{
		InitializeComponent();
		DataContext = company;

		GenerateColumns();
	}

	private void GenerateColumns()
	{
		var columns = GetColumnsForAttributeHelper.GetColumnsForAttribute<EntityModel>(typeof(RequiredForCompanyTableAttribute));
		foreach (var column in columns)
		{
			MyDataGrid.Columns.Add(column);
		}
	}
}