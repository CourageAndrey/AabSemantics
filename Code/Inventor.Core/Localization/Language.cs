using System;
using System.Xml.Serialization;

namespace Inventor.Core.Localization
{
	[Serializable, XmlType(RootName), XmlRoot(RootName)]
	public class Language : ILanguage
	{
		#region Constants

		[XmlIgnore]
		internal const String RootName = "Language";
		[XmlIgnore]
		private const String AttributeName = "Name";
		[XmlIgnore]
		private const String AttributeCulture = "Culture";
		[XmlIgnore]
		private const String DefaultCulture = "en-US";
		[XmlIgnore]
		private const String DefaultName = "English";
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
		[XmlIgnore]
		private const String ElementQuestionNames = "QuestionNames";
		[XmlIgnore]
		private const String ElementAnswers = "Answers";
		[XmlIgnore]
		private const String ElementAttributes = "Attributes";
		[XmlIgnore]
		private const String ElementSystemConceptNames = "SystemConceptNames";
		[XmlIgnore]
		private const String ElementSystemConceptHints = "SystemConceptHints";

		[XmlIgnore]
		private const String ElementConsistency = "Consistency";

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

		[XmlElement(ElementQuestionNames)]
		public LanguageQuestionNames QuestionNamesXml
		{ get; set; }

		[XmlElement(ElementAnswers)]
		public LanguageAnswers AnswersXml
		{ get; set; }

		[XmlElement(ElementAttributes)]
		public LanguageAttributes AttributesXml
		{ get; set; }

		[XmlElement(ElementSystemConceptNames)]
		public LanguageSystemConcepts SystemConceptNamesXml
		{ get; set; }

		[XmlElement(ElementSystemConceptHints)]
		public LanguageSystemConcepts SystemConceptHintsXml
		{ get; set; }

		[XmlElement(ElementConsistency)]
		public LanguageConsistency ConsistencyXml
		{ get; set; }

		#endregion

		#region Interface Properties

		[XmlIgnore]
		public String FileName
		{ get; protected set; }

		[XmlAttribute(AttributeName)]
		public String Name
		{ get; set; }

		[XmlAttribute(AttributeCulture)]
		public String Culture
		{ get; set; }

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

		[XmlIgnore]
		public ILanguageQuestionNames QuestionNames
		{ get { return QuestionNamesXml; } }

		[XmlIgnore]
		public ILanguageAnswers Answers
		{ get { return AnswersXml; } }

		[XmlIgnore]
		public ILanguageAttributes Attributes
		{ get { return AttributesXml; } }

		[XmlIgnore]
		public ILanguageSystemConcepts SystemConceptNames
		{ get { return SystemConceptNamesXml; } }

		[XmlIgnore]
		public ILanguageSystemConcepts SystemConceptHints
		{ get { return SystemConceptHintsXml; } }

		[XmlIgnore]
		public ILanguageConsistency Consistency
		{ get { return ConsistencyXml; } }

		#endregion

		[XmlIgnore]
		public static Language Default
		{ get; protected set; }

		static Language()
		{
			Default = new Language
			{
				FileName = String.Empty,
				Name = DefaultName,
				Culture = DefaultCulture,

				StatementNamesXml = LanguageStatementsPart.CreateDefaultNames(),
				StatementHintsXml = LanguageStatementsPart.CreateDefaultHints(),
				TrueStatementFormatStringsXml = LanguageStatementsPart.CreateDefaultTrue(),
				FalseStatementFormatStringsXml = LanguageStatementsPart.CreateDefaultFalse(),
				QuestionStatementFormatStringsXml = LanguageStatementsPart.CreateDefaultQuestion(),
				QuestionNamesXml = LanguageQuestionNames.CreateDefault(),
				AnswersXml = LanguageAnswers.CreateDefault(),
				AttributesXml = LanguageAttributes.CreateDefault(),
				SystemConceptNamesXml = LanguageSystemConcepts.CreateDefaultNames(),
				SystemConceptHintsXml = LanguageSystemConcepts.CreateDefaultHints(),
				ConsistencyXml = LanguageConsistency.CreateDefault(),
			};
		}

		public override String ToString()
		{
			return Name;
		}
	}
}
