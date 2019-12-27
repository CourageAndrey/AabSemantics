using System;
using System.Collections.Generic;

namespace Sef.Xml
{
	public interface IXmlConvertor
	{
		String ConvertToString(Object value);

		Object ParseString(String stringValue);

		ICollection<Type> GetSupportedTypes();
	}

	public interface IXmlConvertor<T> : IXmlConvertor
	{
		String ConvertEx(T value);

		T ParseEx(String stringValue);
	}
}
