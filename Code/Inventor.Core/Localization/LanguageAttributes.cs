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

		[XmlElement]
		public String IsBoolean
		{ get; set; }

		[XmlElement]
		public String IsComparisonSign
		{ get; set; }

		[XmlElement]
		public String IsSequenceSign
		{ get; set; }

		#endregion

		internal static LanguageAttributes CreateDefault()
		{
			return new LanguageAttributes
			{
				None = "None...",
				IsSign = "Is Sign",
				IsValue = "Is Value",
				IsProcess = "Is Process",
				IsBoolean = "Is Boolean",
				IsComparisonSign = "Is Comparison Sign",
				IsSequenceSign = "Is Processes Sequence Sign",
			};
		}
	}
}
