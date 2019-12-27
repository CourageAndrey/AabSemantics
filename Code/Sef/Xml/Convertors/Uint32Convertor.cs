using System;
using System.Xml;

namespace Sef.Xml.Convertors
{
	internal sealed class Uint32Convertor : XmlConvertorBase<UInt32>
	{
		public override String ConvertEx(UInt32 value)
		{
			return XmlConvert.ToString(value);
		}

		public override UInt32 ParseEx(String stringValue)
		{
			return XmlConvert.ToUInt32(stringValue);
		}
	}
}
