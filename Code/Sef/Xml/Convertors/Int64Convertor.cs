using System;
using System.Xml;

namespace Sef.Xml.Convertors
{
	internal sealed class Int64Convertor : XmlConvertorBase<Int64>
	{
		public override String ConvertEx(Int64 value)
		{
			return XmlConvert.ToString(value);
		}

		public override Int64 ParseEx(String stringValue)
		{
			return XmlConvert.ToInt64(stringValue);
		}
	}
}
