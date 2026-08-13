using System.Xml.Serialization;

using AabSemantics.Localization;

namespace AabSemantics.Modules.Classification.Localization
{
	/// <summary>Statement wordings contributed by the classification module.</summary>
	public interface ILanguageStatements : ILanguageExtensionStatements<ILanguageStatementsPart>
	{
		/// <summary>Wordings used when reporting the module's consistency problems.</summary>
		ILanguageConsistency Consistency
		{ get; }
	}

	/// <summary>Serializable <see cref="ILanguageStatements"/>, loaded from a language file.</summary>
	[XmlType("ClassificationStatements")]
	public class LanguageStatements : ILanguageStatements
	{
		#region Xml Properties

		/// <summary>Display names, in serializable form.</summary>
		[XmlElement(nameof(Names))]
		public LanguageStatementsPart NamesXml
		{ get; set; }

		/// <summary>Tooltip texts, in serializable form.</summary>
		[XmlElement(nameof(Hints))]
		public LanguageStatementsPart HintsXml
		{ get; set; }

		/// <summary>Affirmative wordings, in serializable form.</summary>
		[XmlElement(nameof(TrueFormatStrings))]
		public LanguageStatementsPart TrueFormatStringsXml
		{ get; set; }

		/// <summary>Negative wordings, in serializable form.</summary>
		[XmlElement(nameof(FalseFormatStrings))]
		public LanguageStatementsPart FalseFormatStringsXml
		{ get; set; }

		/// <summary>Interrogative wordings, in serializable form.</summary>
		[XmlElement(nameof(QuestionFormatStrings))]
		public LanguageStatementsPart QuestionFormatStringsXml
		{ get; set; }

		/// <summary>Consistency wordings, in serializable form.</summary>
		[XmlElement(nameof(Consistency))]
		public LanguageConsistency ConsistencyXml
		{ get; set; }

		#endregion

		#region Interface Properties

		/// <summary>Display names.</summary>
		[XmlIgnore]
		public ILanguageStatementsPart Names
		{ get { return NamesXml; } }

		/// <summary>Tooltip texts.</summary>
		[XmlIgnore]
		public ILanguageStatementsPart Hints
		{ get { return HintsXml; } }

		/// <summary>Affirmative wordings.</summary>
		[XmlIgnore]
		public ILanguageStatementsPart TrueFormatStrings
		{ get { return TrueFormatStringsXml; } }

		/// <summary>Negative wordings.</summary>
		[XmlIgnore]
		public ILanguageStatementsPart FalseFormatStrings
		{ get { return FalseFormatStringsXml; } }

		/// <summary>Interrogative wordings.</summary>
		[XmlIgnore]
		public ILanguageStatementsPart QuestionFormatStrings
		{ get { return QuestionFormatStringsXml; } }

		/// <summary>Consistency wordings.</summary>
		[XmlIgnore]
		public ILanguageConsistency Consistency
		{ get { return ConsistencyXml; } }

		#endregion

		/// <summary>Builds this bundle with its built-in English texts.</summary>
		/// <returns>A populated bundle.</returns>
		internal static LanguageStatements CreateDefault()
		{
			return new LanguageStatements
			{
				NamesXml = LanguageStatementsPart.CreateDefaultNames(),
				HintsXml = LanguageStatementsPart.CreateDefaultHints(),
				TrueFormatStringsXml = LanguageStatementsPart.CreateDefaultTrue(),
				FalseFormatStringsXml = LanguageStatementsPart.CreateDefaultFalse(),
				QuestionFormatStringsXml = LanguageStatementsPart.CreateDefaultQuestion(),
				ConsistencyXml = LanguageConsistency.CreateDefault(),
			};
		}
	}
}
