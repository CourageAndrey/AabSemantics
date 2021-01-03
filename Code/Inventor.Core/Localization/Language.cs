using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Xml.Serialization;

namespace Inventor.Core.Localization
{
	public interface ILanguage
	{
		String Name
		{ get; }

		String Culture
		{ get; }

		ILanguageCommon Common
		{ get; }

		ILanguageErrors Errors
		{ get; }

		ILanguageStatements StatementNames
		{ get; }

		ILanguageStatements StatementHints
		{ get; }

		ILanguageStatementFormatStrings TrueStatementFormatStrings
		{ get; }

		ILanguageStatementFormatStrings FalseStatementFormatStrings
		{ get; }

		ILanguageStatementFormatStrings QuestionStatementFormatStrings
		{ get; }

		ILanguageQuestionNames QuestionNames
		{ get; }

		ILanguageAnswers Answers
		{ get; }

		ILanguageUi Ui
		{ get; }

		ILanguageErrorsInventor ErrorsInventor
		{ get; }

		ILanguageConfiguration Configuration
		{ get; }

		ILanguageMisc Misc
		{ get; }
	}

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
		private const string ElementStatementNames = "StatementNames";
		[XmlIgnore]
		private const string ElementStatementHints = "StatementHints";
		[XmlIgnore]
		private const string ElementStatementTrueFormatStrings = "StatementTrueFormatStrings";
		[XmlIgnore]
		private const string ElementStatementFalseFormatStrings = "StatementFalseFormatStrings";
		[XmlIgnore]
		private const string ElementStatementQuestionFormatStrings = "StatementQuestionFormatStrings";
		[XmlIgnore]
		private const string ElementQuestionNames = "QuestionNames";
		[XmlIgnore]
		private const string ElementAnswers = "Answers";
		[XmlIgnore]
		private const string ElementUi = "Ui";
		[XmlIgnore]
		private const string ElementErrorsInventor = "ErrorsInventor";
		[XmlIgnore]
		private const string ElementConfiguration = "Configuration";
		[XmlIgnore]
		private const string ElementMisc = "Misc";

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
		public static Language Current
		{ get; protected internal set; }

		[XmlIgnore]
		public static Language Default
		{ get; protected set; }

		static Language()
		{
			Current = Default = new Language
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

		public static List<LanguageT> LoadAdditional<LanguageT>(String applicationPath)
			where LanguageT : Language, new()
		{
			var languagesFolder = new DirectoryInfo(Path.Combine(applicationPath, FolderPath));
			if (languagesFolder.Exists)
			{
				var languageFiles = languagesFolder.GetFiles(FileFormat);
				return languageFiles.Length > 0
					? languageFiles.Select(f => f.FullName.DeserializeFromFile<LanguageT>()).ToList()
					: new List<LanguageT>();
			}
			else
			{
				languagesFolder.Create();
				return new List<LanguageT>();
			}
		}

		public static LanguageT FindAppropriate<LanguageT>(IEnumerable<LanguageT> all, LanguageT defaultValue)
			where LanguageT : Language
		{
			return all.FirstOrDefault(l => l.Culture == Thread.CurrentThread.CurrentUICulture.Name) ?? defaultValue;
		}

		#endregion

		public override string ToString()
		{
			return Name;
		}
	}

	public class Localizator : ILanguage
	{
		public string Name
		{
			get
			{
				return Language.Current != null
					? Language.Current.Name
					: null;
			}
		}

		public string Culture
		{
			get
			{
				return Language.Current != null
					? Language.Current.Culture
					: null;
			}
		}

		public ILanguageCommon Common
		{
			get
			{
				return Language.Current != null
					? Language.Current.Common
					: null;
			}
		}

		public ILanguageErrors Errors
		{
			get
			{
				return Language.Current != null
					? Language.Current.Errors
					: null;
			}
		}

		public ILanguageStatements StatementNames
		{
			get
			{
				return Language.Current != null
					? Language.Current.StatementNames
					: null;
			}
		}

		public ILanguageStatements StatementHints
		{
			get
			{
				return Language.Current != null
					? Language.Current.StatementHints
					: null;
			}
		}

		public ILanguageStatementFormatStrings TrueStatementFormatStrings
		{
			get
			{
				return Language.Current != null
					? Language.Current.TrueStatementFormatStrings
					: null;
			}
		}

		public ILanguageStatementFormatStrings FalseStatementFormatStrings
		{
			get
			{
				return Language.Current != null
					? Language.Current.FalseStatementFormatStrings
					: null;
			}
		}

		public ILanguageStatementFormatStrings QuestionStatementFormatStrings
		{
			get
			{
				return Language.Current != null
					? Language.Current.QuestionStatementFormatStrings
					: null;
			}
		}

		public ILanguageQuestionNames QuestionNames
		{
			get
			{
				return Language.Current != null
					? Language.Current.QuestionNames
					: null;
			}
		}

		public ILanguageAnswers Answers
		{
			get
			{
				return Language.Current != null
					? Language.Current.Answers
					: null;
			}
		}

		public ILanguageUi Ui
		{
			get
			{
				return Language.Current != null
					? Language.Current.Ui
					: null;
			}
		}

		public ILanguageErrorsInventor ErrorsInventor
		{
			get
			{
				return Language.Current != null
					? Language.Current.ErrorsInventor
					: null;
			}
		}

		public ILanguageConfiguration Configuration
		{
			get
			{
				return Language.Current != null
					? Language.Current.Configuration
					: null;
			}
		}

		public ILanguageMisc Misc
		{
			get
			{
				return Language.Current != null
					? Language.Current.Misc
					: null;
			}
		}
	}
}
