using System;
using System.Xml.Serialization;

namespace Inventor.Core.Localization
{
	public class LanguageAttributes : ILanguageAttributes
	{
		#region Properties

		[XmlElement]
		public String None
		{ get; set; }

		[XmlElement]
		public String IsSign
		{ get; set; }

		[XmlElement]
		public String IsValue
		{ get; set; }

		[XmlElement]
		public String IsProcess
		{ get; set; }

		#endregion

		internal static LanguageAttributes CreateDefault()
		{
			return new LanguageAttributes
			{
				None = "Не заданы...",
				IsSign = "Является признаком",
				IsValue = "Является значением",
				IsProcess = "Является процессом",
			};
		}
	}
}
