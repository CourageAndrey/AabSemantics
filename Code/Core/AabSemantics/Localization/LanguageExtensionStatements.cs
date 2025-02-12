using System.Xml.Serialization;

namespace AabSemantics.Localization
{
	public interface ILanguageExtensionStatements
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

		ILanguageExtensionStatementsConsistency Consistency
		{ get; }
	}

	[XmlType]
	public class LanguageExtensionStatements : ILanguageExtensionStatements
	{
		public ILanguageStatementsPart Names
		{ get; set; }

		public ILanguageStatementsPart Hints
		{ get; set; }

		public ILanguageStatementsPart TrueFormatStrings
		{ get; set; }

		public ILanguageStatementsPart FalseFormatStrings
		{ get; set; }

		public ILanguageStatementsPart QuestionFormatStrings
		{ get; set; }

		public ILanguageExtensionStatementsConsistency Consistency
		{ get; set; }
	}

	public interface ILanguageExtensionStatementsConsistency
	{ }

	[XmlType]
	public class LanguageExtensionStatementsConsistency : ILanguageExtensionStatementsConsistency
	{ }

	public interface ILanguageStatementsPart
	{ }

	[XmlType]
	public class LanguageStatementsPart : ILanguageStatementsPart
	{ }
}
