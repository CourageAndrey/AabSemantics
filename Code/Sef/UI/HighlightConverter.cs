using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Sef.UI
{
	[ValueConversion(typeof(Boolean), typeof(Brush))]
    public class HighlightConverter : IValueConverter
	{
        public Brush HighlighBrush
        { get; set; }

        public Brush DefaultBrush
        { get; set; }

		public Object Convert(Object value, Type targetType, Object parameter, CultureInfo culture)
		{
		    return (Boolean) value ? HighlighBrush : DefaultBrush;
		}

		public Object ConvertBack(Object value, Type targetType, Object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}
