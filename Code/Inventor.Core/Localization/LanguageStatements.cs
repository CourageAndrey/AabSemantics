using System.Xml.Serialization;

namespace Inventor.Core.Localization
{
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
			};
		}
	}
}
