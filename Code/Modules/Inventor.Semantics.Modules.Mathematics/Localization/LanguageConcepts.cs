using System.Xml.Serialization;

namespace Inventor.Semantics.Modules.Mathematics.Localization
{
	public interface ILanguageConcepts
	{
		ILanguageSystemConcepts SystemConceptNames
		{ get; }

		ILanguageSystemConcepts SystemConceptHints
		{ get; }
	}

	[XmlType("MathematicsConcepts")]
	public class LanguageConcepts : ILanguageConcepts
	{
		#region Xml Properties

		[XmlElement(nameof(SystemConceptNames))]
		public LanguageSystemConcepts SystemConceptNamesXml
		{ get; set; }

		[XmlElement(nameof(SystemConceptHints))]
		public LanguageSystemConcepts SystemConceptHintsXml
		{ get; set; }

		#endregion

		#region Interface Properties

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
				SystemConceptNamesXml = LanguageSystemConcepts.CreateDefaultNames(),
				SystemConceptHintsXml = LanguageSystemConcepts.CreateDefaultHints(),
			};
		}
	}
}
