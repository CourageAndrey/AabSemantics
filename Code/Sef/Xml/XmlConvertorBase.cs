using System;
using System.Collections.Generic;

namespace Sef.Xml
{
	public abstract class XmlConvertorBase<T> : IXmlConvertor<T>
	{
		#region Implementation of IXmlConvertor

		public String ConvertToString(Object value)
		{
            return ConvertEx((T) value);
		}

		public Object ParseString(String stringValue)
		{
			return ParseEx(stringValue);
		}

		public ICollection<Type> GetSupportedTypes()
		{
			return (new List<Type> { typeof(T) }).AsReadOnly();
		}

		#endregion

		#region Implementation of IXmlConvertor<T>

		public abstract String ConvertEx(T value);

		public abstract T ParseEx(String stringValue);

		#endregion
	}
}
