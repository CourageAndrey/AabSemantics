using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Inventor.Semantics.Modules.WPF.Converters
{
	[ValueConversion(typeof (Image), typeof (System.Windows.Media.ImageSource))]
	public class ImageConverter : IValueConverter
	{
		public Object Convert(Object value, Type targetType, Object parameter, CultureInfo culture)
		{
			var image = value as Image;
			return image != null ? image.ToSource() : null;
		}

		public Object ConvertBack(Object value, Type targetType, Object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}

	public static class ImageConverterHelper
	{
		public static System.Windows.Media.ImageSource ToSource(this Image image)
		{
			var bitmap = new BitmapImage();
			bitmap.BeginInit();
			var memoryStream = new MemoryStream();
			image.Save(memoryStream, image.RawFormat);
			memoryStream.Seek(0, SeekOrigin.Begin);
			bitmap.StreamSource = memoryStream;
			bitmap.EndInit();
			return bitmap;
		}
	}
}
