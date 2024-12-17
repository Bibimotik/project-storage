using System.Globalization;
using System.Windows;
using System.Windows.Data;


namespace application.Utilities.Converter;

internal class BoolToRadioButtonStyleConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		// Если RadioButton выбран, применим стиль для активного состояния
		if (value is bool isSelected)
		{
			return isSelected ? Application.Current.Resources["SelectedMenuRadioButton"] : Application.Current.Resources["MenuRadioButton"];
		}
		return Application.Current.Resources["MenuRadioButton"];
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}