using System;
using System.Xml;

namespace Sef.Xml.Convertors
{
	internal sealed class DoubleConvertor : XmlConvertorBase<Double>
	{
		public override String ConvertEx(Double value)
		{
			return XmlConvert.ToString(value);
		}

		public override Double ParseEx(String stringValue)
		{
			return XmlConvert.ToDouble(stringValue);
		}
	}
}
