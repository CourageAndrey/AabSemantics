using System;
using System.Xml;

namespace Sef.Xml.Convertors
{
	internal sealed class ByteConvertor : XmlConvertorBase<Byte>
	{
		public override String ConvertEx(Byte value)
		{
			return XmlConvert.ToString(value);
		}

		public override Byte ParseEx(String stringValue)
		{
			return XmlConvert.ToByte(stringValue);
		}
	}
}
