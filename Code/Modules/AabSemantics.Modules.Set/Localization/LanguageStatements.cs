using System.Xml.Serialization;

namespace AabSemantics.Modules.Set.Localization
{
	public interface ILanguageStatements
	{
		ILanguageStatementsPart Names
		{ get; }

		ILanguageStatementsPart Hints
		{ get; }

		ILanguageStatementsPart TrueFormatStrings
		{ get; }

		ILanguageStatementsPart FalseFormatStrings
		{ get; }

		ILanguageStatementsPart QuestionFormatStrings
		{ get; }

		ILanguageConsistency Consistency
		{ get; }
	}

	[XmlType("SetsStatements")]
	public class LanguageStatements : ILanguageStatements
	{
		#region Xml Properties

		[XmlElement(nameof(Names))]
		public LanguageStatementsPart NamesXml
		{ get; set; }

		[XmlElement(nameof(Hints))]
		public LanguageStatementsPart HintsXml
		{ get; set; }

		[XmlElement(nameof(TrueFormatStrings))]
		public LanguageStatementsPart TrueFormatStringsXml
		{ get; set; }

		[XmlElement(nameof(FalseFormatStrings))]
		public LanguageStatementsPart FalseFormatStringsXml
		{ get; set; }

		[XmlElement(nameof(QuestionFormatStrings))]
		public LanguageStatementsPart QuestionFormatStringsXml
		{ get; set; }

		[XmlElement(nameof(Consistency))]
		public LanguageConsistency ConsistencyXml
		{ get; set; }

		#endregion

		#region Interface Properties

		[XmlIgnore]
		public ILanguageStatementsPart Names
		{ get { return NamesXml; } }

		[XmlIgnore]
		public ILanguageStatementsPart Hints
		{ get { return HintsXml; } }

		[XmlIgnore]
		public ILanguageStatementsPart TrueFormatStrings
		{ get { return TrueFormatStringsXml; } }

		[XmlIgnore]
		public ILanguageStatementsPart FalseFormatStrings
		{ get { return FalseFormatStringsXml; } }

		[XmlIgnore]
		public ILanguageStatementsPart QuestionFormatStrings
		{ get { return QuestionFormatStringsXml; } }

		[XmlIgnore]
		public ILanguageConsistency Consistency
		{ get { return ConsistencyXml; } }

		#endregion

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
