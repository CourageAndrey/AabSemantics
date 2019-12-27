using System;
using System.Xml;

namespace Sef.Xml.Convertors
{
	internal sealed class BoolConvertor : XmlConvertorBase<Boolean>
	{
		public override String ConvertEx(Boolean value)
		{
			return XmlConvert.ToString(value);
		}

		public override Boolean ParseEx(String stringValue)
		{
			return XmlConvert.ToBoolean(stringValue);
		}
	}
}
