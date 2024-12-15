using System.Windows.Controls;
using System.Windows.Data;

namespace application.Utilities;

public static class TableHelper
{
	// Метод для получения колонок с атрибутом
	public static List<DataGridColumn> GetColumnsForAttribute<T>(Type attributeType)
	{
		var columns = new List<DataGridColumn>();

		var properties = typeof(T).GetProperties()
			.Where(prop => prop.GetCustomAttributes(attributeType, true).Any());

		foreach (var property in properties)
		{
			var column = new DataGridTextColumn
			{
				Header = property.Name,
				Binding = new Binding(property.Name)
			};

			columns.Add(column);
		}

		return columns;
	}
}