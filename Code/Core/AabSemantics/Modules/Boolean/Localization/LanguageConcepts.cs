using System.Xml.Serialization;

using AabSemantics.Localization;

namespace AabSemantics.Modules.Boolean.Localization
{
	/// <summary>Concept names and hints contributed by the boolean module.</summary>
	public interface ILanguageConcepts : ILanguageExtensionConcepts
	{
		/// <summary>Display names of the logical value concepts.</summary>
		ILanguageSystemConcepts SystemConceptNames
		{ get; }

		/// <summary>Tooltip texts of the logical value concepts.</summary>
		ILanguageSystemConcepts SystemConceptHints
		{ get; }
	}

	/// <summary>Serializable <see cref="ILanguageConcepts"/>, loaded from a language file.</summary>
	[XmlType("BooleanConcepts")]
	public class LanguageConcepts : ILanguageConcepts
	{
		#region Xml Properties

		/// <summary>Concept names, in serializable form.</summary>
		[XmlElement(nameof(SystemConceptNames))]
		public LanguageSystemConcepts SystemConceptNamesXml
		{ get; set; }

		/// <summary>Concept hints, in serializable form.</summary>
		[XmlElement(nameof(SystemConceptHints))]
		public LanguageSystemConcepts SystemConceptHintsXml
		{ get; set; }

		#endregion

		#region Interface Properties

		/// <summary>Display names of the logical value concepts.</summary>
		[XmlIgnore]
		public ILanguageSystemConcepts SystemConceptNames
		{ get { return SystemConceptNamesXml; } }

		/// <summary>Tooltip texts of the logical value concepts.</summary>
		[XmlIgnore]
		public ILanguageSystemConcepts SystemConceptHints
		{ get { return SystemConceptHintsXml; } }

		#endregion

		/// <summary>Builds this bundle with its built-in English texts.</summary>
		/// <returns>A populated bundle.</returns>
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
