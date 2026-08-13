using System;
using System.Globalization;
using System.Windows.Data;

namespace AabSemantics.Extensions.WPF.Converters
{
	/// <summary>Binding converter rendering an <see cref="INamed"/> item's name in the current language.</summary>
	public class NamedConverter : IValueConverter
	{
		/// <summary>Language the names are rendered in.</summary>
		public ILanguage Language
		{ get; set; }

		/// <summary>Converts a bound value for display.</summary>
		/// <param name="value">Value to convert.</param>
		/// <param name="targetType">Type the binding expects.</param>
		/// <param name="parameter">Converter parameter; unused.</param>
		/// <param name="culture">Culture of the binding.</param>
		/// <returns>The converted value.</returns>
		public Object Convert(Object value, Type targetType, Object parameter, CultureInfo culture)
		{
			return ((INamed) value).Name.GetValue(Language);
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
}
