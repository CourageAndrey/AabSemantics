using System;
using System.Xml;

namespace Sef.Xml.Convertors
{
	internal sealed class Uint64Convertor : XmlConvertorBase<UInt64>
	{
		public override String ConvertEx(UInt64 value)
		{
			return XmlConvert.ToString(value);
		}

		public override UInt64 ParseEx(String stringValue)
		{
			return XmlConvert.ToUInt64(stringValue);
		}
	}
}
