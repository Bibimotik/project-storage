using System.IO;
using System.Windows.Media.Imaging;

namespace application.Utilities;

public class ImageHelper
{
	public static byte[] ConvertImageToByteArray(string imagePath)
	{
		using (var fileStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read))
		{
			using (var memoryStream = new MemoryStream())
			{
				fileStream.CopyTo(memoryStream);
				return memoryStream.ToArray();
			}
		}
	}
}