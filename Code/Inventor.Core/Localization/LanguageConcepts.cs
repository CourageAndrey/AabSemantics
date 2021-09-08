using System.Xml.Serialization;

namespace Inventor.Core.Localization
{
	public class LanguageConcepts : ILanguageConcepts
	{
		#region Xml Properties

		[XmlElement(nameof(Attributes))]
		public LanguageAttributes AttributesXml
		{ get; set; }

		[XmlElement(nameof(SystemConceptNames))]
		public LanguageSystemConcepts SystemConceptNamesXml
		{ get; set; }

		[XmlElement(nameof(SystemConceptHints))]
		public LanguageSystemConcepts SystemConceptHintsXml
		{ get; set; }

		#endregion

		#region Interface Properties

		[XmlIgnore]
		public ILanguageAttributes Attributes
		{ get { return AttributesXml; } }

		[XmlIgnore]
		public ILanguageSystemConcepts SystemConceptNames
		{ get { return SystemConceptNamesXml; } }

		[XmlIgnore]
		public ILanguageSystemConcepts SystemConceptHints
		{ get { return SystemConceptHintsXml; } }

		#endregion

		internal static LanguageConcepts CreateDefault()
		{
			return new LanguageConcepts
			{
				AttributesXml = LanguageAttributes.CreateDefault(),
				SystemConceptNamesXml = LanguageSystemConcepts.CreateDefaultNames(),
				SystemConceptHintsXml = LanguageSystemConcepts.CreateDefaultHints(),
			};
		}
	}
}
