using System.Xml.Serialization;

namespace Samples.Semantics.Sample07.CustomModule.Localization
{
	public interface ILanguageAttributes
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
