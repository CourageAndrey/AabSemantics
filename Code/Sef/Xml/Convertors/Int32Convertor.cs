using System;
using System.Xml;

namespace Sef.Xml.Convertors
{
	internal sealed class Int32Convertor : XmlConvertorBase<Int32>
	{
		public override String ConvertEx(Int32 value)
		{
			return XmlConvert.ToString(value);
		}

		public override Int32 ParseEx(String stringValue)
		{
			return XmlConvert.ToInt32(stringValue);
		}
	}
}
