using System;
using System.Xml;

namespace Sef.Xml.Convertors
{
	internal sealed class DecimalConvertor : XmlConvertorBase<Decimal>
	{
		public override String ConvertEx(Decimal value)
		{
			return XmlConvert.ToString(value);
		}

		public override Decimal ParseEx(String stringValue)
		{
			return XmlConvert.ToDecimal(stringValue);
		}
	}
}
