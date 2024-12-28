using System;
using System.Xml.Serialization;

namespace AabSemantics.Modules.NaturalScience.Localization
{
	public interface ILanguageAttributes
	{
		String IsChemicalElement
		{ get; }
	}

	[XmlType("NaturalScienseAttributes")]
	public class LanguageAttributes : ILanguageAttributes
	{
		#region Properties

		[XmlElement]
		public String IsChemicalElement
		{ get; set; }

		#endregion

		internal static LanguageAttributes CreateDefault()
		{
			return new LanguageAttributes
			{
				IsChemicalElement = "Is Chemical Element",
			};
		}
	}
}
