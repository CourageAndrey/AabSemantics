using System;
using System.Xml.Serialization;

namespace Inventor.Core.Localization
{
	public class LanguageStatements : ILanguageStatements
	{
		#region Constants

		[XmlIgnore]
		private const String ElementStatementNames = "StatementNames";
		[XmlIgnore]
		private const String ElementStatementHints = "StatementHints";
		[XmlIgnore]
		private const String ElementStatementTrueFormatStrings = "StatementTrueFormatStrings";
		[XmlIgnore]
		private const String ElementStatementFalseFormatStrings = "StatementFalseFormatStrings";
		[XmlIgnore]
		private const String ElementStatementQuestionFormatStrings = "StatementQuestionFormatStrings";

		#endregion

		#region Xml Properties

		[XmlElement(ElementStatementNames)]
		public LanguageStatementsPart StatementNamesXml
		{ get; set; }

		[XmlElement(ElementStatementHints)]
		public LanguageStatementsPart StatementHintsXml
		{ get; set; }

		[XmlElement(ElementStatementTrueFormatStrings)]
		public LanguageStatementsPart TrueStatementFormatStringsXml
		{ get; set; }

		[XmlElement(ElementStatementFalseFormatStrings)]
		public LanguageStatementsPart FalseStatementFormatStringsXml
		{ get; set; }

		[XmlElement(ElementStatementQuestionFormatStrings)]
		public LanguageStatementsPart QuestionStatementFormatStringsXml
		{ get; set; }

		#endregion

		#region Interface Properties

		[XmlIgnore]
		public ILanguageStatementsPart StatementNames
		{ get { return StatementNamesXml; } }

		[XmlIgnore]
		public ILanguageStatementsPart StatementHints
		{ get { return StatementHintsXml; } }

		[XmlIgnore]
		public ILanguageStatementsPart TrueStatementFormatStrings
		{ get { return TrueStatementFormatStringsXml; } }

		[XmlIgnore]
		public ILanguageStatementsPart FalseStatementFormatStrings
		{ get { return FalseStatementFormatStringsXml; } }

		[XmlIgnore]
		public ILanguageStatementsPart QuestionStatementFormatStrings
		{ get { return QuestionStatementFormatStringsXml; } }

		#endregion

		internal static LanguageStatements CreateDefault()
		{
			return new LanguageStatements
			{
				StatementNamesXml = LanguageStatementsPart.CreateDefaultNames(),
				StatementHintsXml = LanguageStatementsPart.CreateDefaultHints(),
				TrueStatementFormatStringsXml = LanguageStatementsPart.CreateDefaultTrue(),
				FalseStatementFormatStringsXml = LanguageStatementsPart.CreateDefaultFalse(),
				QuestionStatementFormatStringsXml = LanguageStatementsPart.CreateDefaultQuestion(),
			};
		}
	}
}
