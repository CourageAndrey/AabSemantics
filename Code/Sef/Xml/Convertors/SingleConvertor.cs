using System;
using System.Xml;

namespace Sef.Xml.Convertors
{
	internal sealed class SingleConvertor : XmlConvertorBase<Single>
	{
		public override String ConvertEx(Single value)
		{
			return XmlConvert.ToString(value);
		}

		public override Single ParseEx(String stringValue)
		{
			return XmlConvert.ToSingle(stringValue);
		}
	}
}
