using System;
using System.Xml;

namespace Sef.Xml.Convertors
{
	internal sealed class SbyteConvertor : XmlConvertorBase<SByte>
	{
		public override String ConvertEx(SByte value)
		{
			return XmlConvert.ToString(value);
		}

		public override SByte ParseEx(String stringValue)
		{
			return XmlConvert.ToSByte(stringValue);
		}
	}
}
