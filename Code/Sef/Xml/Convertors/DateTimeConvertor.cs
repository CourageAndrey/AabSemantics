using System;
using System.Xml;

namespace Sef.Xml.Convertors
{
	internal sealed class DateTimeConvertor : XmlConvertorBase<DateTime>
	{
		public override String ConvertEx(DateTime value)
		{
			return XmlConvert.ToString(value, XmlDateTimeSerializationMode.Local);
		}

		public override DateTime ParseEx(String stringValue)
		{
			return XmlConvert.ToDateTime(stringValue, XmlDateTimeSerializationMode.Local);
		}
	}
}
