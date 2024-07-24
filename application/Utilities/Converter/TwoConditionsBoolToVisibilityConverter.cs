using System.Globalization;
using System.Windows.Data;
using System.Windows;

namespace application.Utilities.Converter;
public class TwoConditionsBoolToVisibilityConverter : IMultiValueConverter
{
	public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
	{
		if (values.Length == 2 && values[0] is bool condition1 && values[1] is bool condition2)
		{
			return condition1 && condition2 ? Visibility.Visible : Visibility.Collapsed;
		}

		return DependencyProperty.UnsetValue;
	}

	public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
	{
		throw new NotSupportedException("TwoConditionsBoolToVisibilityConverter does not support ConvertBack.");
	}
}
