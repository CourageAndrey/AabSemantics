using System;
using System.Xml.Serialization;

namespace Inventor.Core.Localization
{
	public class LanguageStatements : ILanguageStatements
	{
		#region Constants

		[XmlIgnore]
		private const String ElementStatementNames = "Names";
		[XmlIgnore]
		private const String ElementStatementHints = "Hints";
		[XmlIgnore]
		private const String ElementStatementTrueFormatStrings = "TrueFormatStrings";
		[XmlIgnore]
		private const String ElementStatementFalseFormatStrings = "FalseFormatStrings";
		[XmlIgnore]
		private const String ElementStatementQuestionFormatStrings = "QuestionFormatStrings";

		#endregion

		#region Xml Properties

		[XmlElement(ElementStatementNames)]
		public LanguageStatementsPart NamesXml
		{ get; set; }

		[XmlElement(ElementStatementHints)]
		public LanguageStatementsPart HintsXml
		{ get; set; }

		[XmlElement(ElementStatementTrueFormatStrings)]
		public LanguageStatementsPart TrueFormatStringsXml
		{ get; set; }

		[XmlElement(ElementStatementFalseFormatStrings)]
		public LanguageStatementsPart FalseFormatStringsXml
		{ get; set; }

		[XmlElement(ElementStatementQuestionFormatStrings)]
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
