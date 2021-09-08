using System;
using System.Xml.Serialization;

namespace Inventor.Core.Localization
{
	public class LanguageConcepts : ILanguageConcepts
	{
		#region Constants

		[XmlIgnore]
		private const String ElementAttributes = "Attributes";
		[XmlIgnore]
		private const String ElementSystemConceptNames = "SystemConceptNames";
		[XmlIgnore]
		private const String ElementSystemConceptHints = "SystemConceptHints";

		#endregion

		#region Xml Properties

		[XmlElement(ElementAttributes)]
		public LanguageAttributes AttributesXml
		{ get; set; }

		[XmlElement(ElementSystemConceptNames)]
		public LanguageSystemConcepts SystemConceptNamesXml
		{ get; set; }

		[XmlElement(ElementSystemConceptHints)]
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
