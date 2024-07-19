﻿using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace application.Utilities;
internal class BoolToVisibilityConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value is bool boolValue)
		{
			return boolValue ? Visibility.Visible : Visibility.Collapsed;
		}

		return DependencyProperty.UnsetValue;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value is Visibility visibility)
		{
			return visibility == Visibility.Visible;
		}

		return DependencyProperty.UnsetValue;
	}
}
