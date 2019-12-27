using System;
using System.Xml;

namespace Sef.Xml.Convertors
{
	internal sealed class Uint16Convertor : XmlConvertorBase<UInt16>
	{
		public override String ConvertEx(UInt16 value)
		{
			return XmlConvert.ToString(value);
		}

		public override UInt16 ParseEx(String stringValue)
		{
			return XmlConvert.ToUInt16(stringValue);
		}
	}
}
