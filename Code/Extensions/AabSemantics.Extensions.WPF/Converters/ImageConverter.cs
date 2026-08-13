using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace AabSemantics.Extensions.WPF.Converters
{
	/// <summary>Binding converter turning a WinForms image into something WPF can display.</summary>
	[ValueConversion(typeof (Image), typeof (System.Windows.Media.ImageSource))]
	public class ImageConverter : IValueConverter
	{
		/// <summary>Converts a bound value for display.</summary>
		/// <param name="value">Value to convert.</param>
		/// <param name="targetType">Type the binding expects.</param>
		/// <param name="parameter">Converter parameter; unused.</param>
		/// <param name="culture">Culture of the binding.</param>
		/// <returns>The converted value.</returns>
		public Object Convert(Object value, Type targetType, Object parameter, CultureInfo culture)
		{
			var image = value as Image;
			return image != null ? image.ToSource() : null;
		}

		/// <summary>Not supported: the binding is one-way.</summary>
		/// <param name="value">Value to convert.</param>
		/// <param name="targetType">Type the binding expects.</param>
		/// <param name="parameter">Converter parameter; unused.</param>
		/// <param name="culture">Culture of the binding.</param>
		/// <returns>Never returns normally.</returns>
		/// <exception cref="System.NotSupportedException">Always thrown.</exception>
		public Object ConvertBack(Object value, Type targetType, Object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}

	/// <summary>Image conversion used by both the converter and the tree node icons.</summary>
	public static class ImageConverterHelper
	{
		/// <summary>Converts a WinForms image into a WPF image source.</summary>
		/// <param name="image">Image to convert.</param>
		/// <returns>The converted source.</returns>
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
