using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Xml.Serialization;

using Inventor.Core.Utils;

namespace Inventor.Core.Localization
{
	[Serializable, XmlRoot(RootName)]
	public class Language : ILanguage
	{
		#region Constants

		[XmlIgnore]
		private const String RootName = "Language";
		[XmlIgnore]
		private const String AttributeName = "Name";
		[XmlIgnore]
		private const String AttributeCulture = "Culture";
		[XmlIgnore]
		private const String ElementCommon = "Common";
		[XmlIgnore]
		private const String ElementErrors = "Errors";
		[XmlIgnore]
		private const String ElementEditor = "Editor";
		[XmlIgnore]
		private const String DefaultCulture = "ru-RU";
		[XmlIgnore]
		private const String DefaultName = "Русский";
		[XmlIgnore]
		private const String FileFormat = "*.xml";
		[XmlIgnore]
		private const String FolderPath = "Localization";
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
		private const String ElementUi = "Ui";
		[XmlIgnore]
		private const String ElementErrorsInventor = "ErrorsInventor";
		[XmlIgnore]
		private const String ElementConfiguration = "Configuration";
		[XmlIgnore]
		private const String ElementMisc = "Misc";

		#endregion

		#region Xml Properties

		[XmlElement(ElementCommon)]
		public LanguageCommon CommonXml
		{ get; set; }

		[XmlElement(ElementErrors)]
		public LanguageErrors ErrorsXml
		{ get; set; }

		[XmlElement(ElementStatementNames)]
		public LanguageStatements StatementNamesXml
		{ get; set; }

		[XmlElement(ElementStatementHints)]
		public LanguageStatements StatementHintsXml
		{ get; set; }

		[XmlElement(ElementStatementTrueFormatStrings)]
		public LanguageStatementFormatStrings TrueStatementFormatStringsXml
		{ get; set; }

		[XmlElement(ElementStatementFalseFormatStrings)]
		public LanguageStatementFormatStrings FalseStatementFormatStringsXml
		{ get; set; }

		[XmlElement(ElementStatementQuestionFormatStrings)]
		public LanguageStatementFormatStrings QuestionStatementFormatStringsXml
		{ get; set; }

		[XmlElement(ElementQuestionNames)]
		public LanguageQuestionNames QuestionNamesXml
		{ get; set; }

		[XmlElement(ElementAnswers)]
		public LanguageAnswers AnswersXml
		{ get; set; }

		[XmlElement(ElementUi)]
		public LanguageUi UiXml
		{ get; set; }

		[XmlElement(ElementErrorsInventor)]
		public LanguageErrorsInventor ErrorsInventorXml
		{ get; set; }

		[XmlElement(ElementConfiguration)]
		public LanguageConfiguration ConfigurationXml
		{ get; set; }

		[XmlElement(ElementMisc)]
		public LanguageMisc MiscXml
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
		public ILanguageCommon Common
		{ get { return CommonXml; } }

		[XmlIgnore]
		public ILanguageErrors Errors
		{ get { return ErrorsXml; } }

		[XmlIgnore]
		public ILanguageStatements StatementNames
		{ get { return StatementNamesXml; } }

		[XmlIgnore]
		public ILanguageStatements StatementHints
		{ get { return StatementHintsXml; } }

		[XmlIgnore]
		public ILanguageStatementFormatStrings TrueStatementFormatStrings
		{ get { return TrueStatementFormatStringsXml; } }

		[XmlIgnore]
		public ILanguageStatementFormatStrings FalseStatementFormatStrings
		{ get { return FalseStatementFormatStringsXml; } }

		[XmlIgnore]
		public ILanguageStatementFormatStrings QuestionStatementFormatStrings
		{ get { return QuestionStatementFormatStringsXml; } }

		[XmlIgnore]
		public ILanguageQuestionNames QuestionNames
		{ get { return QuestionNamesXml; } }

		[XmlIgnore]
		public ILanguageAnswers Answers
		{ get { return AnswersXml; } }

		[XmlIgnore]
		public ILanguageUi Ui
		{ get { return UiXml; } }

		[XmlIgnore]
		public ILanguageErrorsInventor ErrorsInventor
		{ get { return ErrorsInventorXml; } }

		[XmlIgnore]
		public ILanguageConfiguration Configuration
		{ get { return ConfigurationXml; } }

		[XmlIgnore]
		public ILanguageMisc Misc
		{ get { return MiscXml; } }

		#endregion

		#region Singleton

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

				CommonXml = LanguageCommon.CreateDefault(),
				ErrorsXml = LanguageErrors.CreateDefault(),

				StatementNamesXml = LanguageStatements.CreateDefaultNames(),
				StatementHintsXml = LanguageStatements.CreateDefaultHints(),
				TrueStatementFormatStringsXml = LanguageStatementFormatStrings.CreateDefaultTrue(),
				FalseStatementFormatStringsXml = LanguageStatementFormatStrings.CreateDefaultFalse(),
				QuestionStatementFormatStringsXml = LanguageStatementFormatStrings.CreateDefaultQuestion(),
				QuestionNamesXml = LanguageQuestionNames.CreateDefault(),
				AnswersXml = LanguageAnswers.CreateDefault(),
				UiXml = LanguageUi.CreateDefault(),
				ErrorsInventorXml = LanguageErrorsInventor.CreateDefault(),
				ConfigurationXml = LanguageConfiguration.CreateDefault(),
				MiscXml = LanguageMisc.CreateDefault(),
			};
		}

		public static ICollection<ILanguage> LoadAdditional(String applicationPath)
		{
			var languagesFolder = new DirectoryInfo(Path.Combine(applicationPath, FolderPath));
			if (languagesFolder.Exists)
			{
				var languageFiles = languagesFolder.GetFiles(FileFormat);
				return languageFiles.Length > 0
					? languageFiles.Select(f => f.FullName.DeserializeFromFile<Language>() as ILanguage).ToList()
					: new List<ILanguage>();
			}
			else
			{
				languagesFolder.Create();
				return new List<ILanguage>();
			}
		}

		#endregion

		public override String ToString()
		{
			return Name;
		}
	}

	public static class LanguageExtensions
	{
		public static ILanguage FindAppropriate(this IEnumerable<ILanguage> languages, ILanguage @default)
		{
			return languages.FirstOrDefault(l => l.Culture == Thread.CurrentThread.CurrentUICulture.Name) ?? @default;
		}
	}

	public class Localizator : ILanguage
	{
		private ILanguage _language;

		public Localizator()
			: this(null)
		{ }

		public Localizator(ILanguage language)
		{
			_language = language;
		}

		#region Properties

		public String Name
		{ get { return _language?.Name; } }

		public String Culture
		{ get { return _language?.Culture; } }

		public ILanguageCommon Common
		{ get { return _language?.Common; } }

		public ILanguageErrors Errors
		{ get { return _language?.Errors; } }

		public ILanguageStatements StatementNames
		{ get { return _language?.StatementNames; } }

		public ILanguageStatements StatementHints
		{ get { return _language?.StatementHints; } }

		public ILanguageStatementFormatStrings TrueStatementFormatStrings
		{ get { return _language?.TrueStatementFormatStrings; } }

		public ILanguageStatementFormatStrings FalseStatementFormatStrings
		{ get { return _language?.FalseStatementFormatStrings; } }

		public ILanguageStatementFormatStrings QuestionStatementFormatStrings
		{ get { return _language?.QuestionStatementFormatStrings; } }

		public ILanguageQuestionNames QuestionNames
		{ get { return _language?.QuestionNames; } }

		public ILanguageAnswers Answers
		{ get { return _language?.Answers; } }

		public ILanguageUi Ui
		{ get { return _language?.Ui; } }

		public ILanguageErrorsInventor ErrorsInventor
		{ get { return _language?.ErrorsInventor; } }

		public ILanguageConfiguration Configuration
		{ get { return _language?.Configuration; } }

		public ILanguageMisc Misc
		{ get { return _language?.Misc; } }

		#endregion

		public void Change(ILanguage language)
		{
			_language = language;
		}
	}
}
