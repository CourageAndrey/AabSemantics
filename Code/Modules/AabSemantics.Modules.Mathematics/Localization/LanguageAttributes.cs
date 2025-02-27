using System;
using System.Xml.Serialization;

using AabSemantics.Localization;

namespace AabSemantics.Modules.Mathematics.Localization
{
	public interface ILanguageAttributes : ILanguageExtensionAttributes
	{
		String IsComparisonSign
		{ get; }
	}

	[XmlType("MathematicsAttributes")]
	public class LanguageAttributes : ILanguageAttributes
	{
		#region Properties

		[XmlElement]
		public String IsComparisonSign
		{ get; set; }

		#endregion

		internal static LanguageAttributes CreateDefault()
		{
			return new LanguageAttributes
			{
				IsComparisonSign = "Is Comparison Sign",
			};
		}
	}
}
