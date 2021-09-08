using System;

namespace Inventor.Client.Localization
{
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

		public Core.ILanguageStatementsPart StatementNames
		{ get { return _language?.StatementNames; } }

		public Core.ILanguageStatementsPart StatementHints
		{ get { return _language?.StatementHints; } }

		public Core.ILanguageStatementsPart TrueStatementFormatStrings
		{ get { return _language?.TrueStatementFormatStrings; } }

		public Core.ILanguageStatementsPart FalseStatementFormatStrings
		{ get { return _language?.FalseStatementFormatStrings; } }

		public Core.ILanguageStatementsPart QuestionStatementFormatStrings
		{ get { return _language?.QuestionStatementFormatStrings; } }

		public Core.ILanguageQuestionNames QuestionNames
		{ get { return _language?.QuestionNames; } }

		public Core.ILanguageAnswers Answers
		{ get { return _language?.Answers; } }

		public Core.ILanguageAttributes Attributes
		{ get { return _language?.Attributes; } }

		public ILanguageUi Ui
		{ get { return _language?.Ui; } }

		public Core.ILanguageSystemConcepts SystemConceptNames
		{ get { return _language?.SystemConceptNames; } }

		public Core.ILanguageSystemConcepts SystemConceptHints
		{ get { return _language?.SystemConceptHints; } }

		public Core.ILanguageConsistency Consistency
		{ get { return _language?.Consistency; } }

		public ILanguageMisc Misc
		{ get { return _language?.Misc; } }

		#endregion

		public void Change(ILanguage language)
		{
			_language = language;
		}
	}
}