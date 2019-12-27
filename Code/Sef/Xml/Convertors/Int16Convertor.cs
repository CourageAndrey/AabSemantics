using System;
using System.Xml;

namespace Sef.Xml.Convertors
{
	internal sealed class Int16Convertor : XmlConvertorBase<Int16>
	{
		public override String ConvertEx(Int16 value)
		{
			return XmlConvert.ToString(value);
		}

		public override Int16 ParseEx(String stringValue)
		{
			return XmlConvert.ToInt16(stringValue);
		}
	}
}
