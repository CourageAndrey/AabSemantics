using System;
using System.Globalization;
using System.Windows.Data;

using Inventor.Semantics;

namespace Inventor.WPF.Converters
{
	public class NamedConverter : IValueConverter
	{
		public ILanguage Language
		{ get; set; }

		public Object Convert(Object value, Type targetType, Object parameter, CultureInfo culture)
		{
			return ((INamed) value).Name.GetValue(Language);
		}

		public Object ConvertBack(Object value, Type targetType, Object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}
