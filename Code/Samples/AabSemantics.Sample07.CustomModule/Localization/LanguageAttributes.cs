using System.Xml.Serialization;

using AabSemantics.Localization;

namespace AabSemantics.Sample07.CustomModule.Localization
{
	public interface ILanguageAttributes : ILanguageExtensionAttributes
	{
		string Custom
		{ get; }
	}

	[XmlType("CustomAttributes")]
	public class LanguageAttributes : ILanguageAttributes
	{
		[XmlElement]
		public string Custom
		{ get; set; }

		internal static LanguageAttributes CreateDefault()
		{
			return new LanguageAttributes
			{
				Custom = "Is custom",
			};
		}
	}
}
