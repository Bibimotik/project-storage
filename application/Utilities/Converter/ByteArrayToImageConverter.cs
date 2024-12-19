using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace application.Utilities.Converter;
public class ByteArrayToImageConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value is byte[] byteArray && byteArray.Length > 0)
		{
			try
			{
				using var stream = new MemoryStream(byteArray);
				var image = new BitmapImage();
				image.BeginInit();
				image.CacheOption = BitmapCacheOption.OnLoad;
				image.StreamSource = stream;
				image.EndInit();
				image.Freeze();
				return image;
			}
			catch
			{
				return null;
			}
		}

		return null;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}