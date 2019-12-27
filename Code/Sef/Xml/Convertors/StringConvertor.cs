using System;
namespace Sef.Xml.Convertors
{
	internal sealed class StringConvertor : XmlConvertorBase<String>
	{
		public override String ConvertEx(String value)
		{
			return value;
		}

		public override String ParseEx(String stringValue)
		{
			return stringValue;
		}
	}
}
