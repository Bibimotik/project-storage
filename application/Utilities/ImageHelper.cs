using System.IO;
using System.Windows.Media.Imaging;

namespace application.Utilities;

public class ImageHelper
{
	public static byte[] ConvertImageToByteArray(string imagePath)
	{
		BitmapImage bitmapImage = new BitmapImage(new Uri(imagePath));
		byte[] data;

		JpegBitmapEncoder encoder = new JpegBitmapEncoder();
		encoder.Frames.Add(BitmapFrame.Create(bitmapImage));
		using (MemoryStream ms = new MemoryStream())
		{
			encoder.Save(ms);
			data = ms.ToArray();
		}

		return data;
	}
}