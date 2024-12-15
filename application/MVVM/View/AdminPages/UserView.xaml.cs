using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
		{
			MyDataGrid.Columns.Add(column);
		}
	}

	private void DataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
	{
		// Убираем ненужные колонки
		//if (
		//	e.PropertyName == "INN" ||
		//	e.PropertyName == "KPP" ||
		//	e.PropertyName == "FullName" ||
		//	e.PropertyName == "ShortName" ||
		//	e.PropertyName == "LegalAddress" ||
		//	e.PropertyName == "PostalAddress" ||
		//	e.PropertyName == "OGRN" ||
		//	e.PropertyName == "Director" ||
		//	e.PropertyName == "Director" ||
		//	e.PropertyName == "ConfirmPassword" ||
		//	e.PropertyName == "Message" ||
		//	e.PropertyName == "Images" ||
		//	e.PropertyName == "Code" ||
		//	e.PropertyName == "InputCode"
		//	)
		//{
		//	e.Cancel = true; // Пропустить этот столбец
		//}
	}
}
