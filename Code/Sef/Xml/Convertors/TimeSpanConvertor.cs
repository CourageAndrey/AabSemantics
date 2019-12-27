using System;
using System.Xml;

namespace Sef.Xml.Convertors
{
	internal sealed class TimeSpanConvertor : XmlConvertorBase<TimeSpan>
	{
		public override String ConvertEx(TimeSpan value)
		{
			return XmlConvert.ToString(value);
		}

		public override TimeSpan ParseEx(String stringValue)
		{
			return XmlConvert.ToTimeSpan(stringValue);
		}
	}
}
