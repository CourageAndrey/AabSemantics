using System;
using System.Xml.Serialization;

using Sef.Localization;

namespace Inventor.Core.Localization
{
	public interface ILanguageEx : ILanguage
	{
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
	public class LanguageEx : Language, ILanguageEx
	{
		#region Xml Properties

		private const string ElementStatementNames = "StatementNames";
		private const string ElementStatementHints = "StatementHints";
		private const string ElementStatementTrueFormatStrings = "StatementTrueFormatStrings";
		private const string ElementStatementFalseFormatStrings = "StatementFalseFormatStrings";
		private const string ElementStatementQuestionFormatStrings = "StatementQuestionFormatStrings";
		private const string ElementQuestionNames = "QuestionNames";
		private const string ElementAnswers = "Answers";
		private const string ElementUi = "Ui";
		private const string ElementErrorsInventor = "ErrorsInventor";
		private const string ElementConfiguration = "Configuration";
		private const string ElementMisc = "Misc";

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

		private static LanguageEx _defaultEx;
		private static LanguageEx _currentEx;

		[XmlIgnore]
		public static LanguageEx CurrentEx
		{
			get { return _currentEx; }
			set { Current = _currentEx = value; }
		}

		[XmlIgnore]
		public static LanguageEx DefaultEx
		{
			get { return _defaultEx; }
			private set { Default = _defaultEx = value; }
		}

		static LanguageEx()
		{
			CurrentEx = DefaultEx = new LanguageEx
			{
				Culture = Default.Culture,
				Name = Default.Name,
				FileName = Default.FileName,
				CommonXml = Default.CommonXml,
				ErrorsXml = Default.ErrorsXml,
				EditorXml = Default.EditorXml,
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

		#endregion
	}

	public sealed class LocalizatorEx : Localizator, ILanguageEx
	{
		public ILanguageStatements StatementNames
		{
			get
			{
				return LanguageEx.CurrentEx != null
					? LanguageEx.CurrentEx.StatementNames
					: null;
			}
		}

		public ILanguageStatements StatementHints
		{
			get
			{
				return LanguageEx.CurrentEx != null
					? LanguageEx.CurrentEx.StatementHints
					: null;
			}
		}

		public ILanguageStatementFormatStrings TrueStatementFormatStrings
		{
			get
			{
				return LanguageEx.CurrentEx != null
					? LanguageEx.CurrentEx.TrueStatementFormatStrings
					: null;
			}
		}

		public ILanguageStatementFormatStrings FalseStatementFormatStrings
		{
			get
			{
				return LanguageEx.CurrentEx != null
					? LanguageEx.CurrentEx.FalseStatementFormatStrings
					: null;
			}
		}

		public ILanguageStatementFormatStrings QuestionStatementFormatStrings
		{
			get
			{
				return LanguageEx.CurrentEx != null
					? LanguageEx.CurrentEx.QuestionStatementFormatStrings
					: null;
			}
		}

		public ILanguageQuestionNames QuestionNames
		{
			get
			{
				return LanguageEx.CurrentEx != null
					? LanguageEx.CurrentEx.QuestionNames
					: null;
			}
		}

		public ILanguageAnswers Answers
		{
			get
			{
				return LanguageEx.CurrentEx != null
					? LanguageEx.CurrentEx.Answers
					: null;
			}
		}

		public ILanguageUi Ui
		{
			get
			{
				return LanguageEx.CurrentEx != null
					? LanguageEx.CurrentEx.Ui
					: null;
			}
		}

		public ILanguageErrorsInventor ErrorsInventor
		{
			get
			{
				return LanguageEx.CurrentEx != null
					? LanguageEx.CurrentEx.ErrorsInventor
					: null;
			}
		}

		public ILanguageConfiguration Configuration
		{
			get
			{
				return LanguageEx.CurrentEx != null
					? LanguageEx.CurrentEx.Configuration
					: null;
			}
		}

		public ILanguageMisc Misc
		{
			get
			{
				return LanguageEx.CurrentEx != null
					? LanguageEx.CurrentEx.Misc
					: null;
			}
		}
	}
}
