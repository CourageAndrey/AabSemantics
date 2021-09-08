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

		public Core.ILanguageStatements Statements
		{ get { return _language?.Statements; } }

		public Core.ILanguageQuestions Questions
		{ get { return _language?.Questions; } }

		public Core.ILanguageConcepts Concepts
		{ get { return _language?.Concepts; } }

		public Core.ILanguageConsistency Consistency
		{ get { return _language?.Consistency; } }

		public ILanguageUi Ui
		{ get { return _language?.Ui; } }

		public ILanguageMisc Misc
		{ get { return _language?.Misc; } }

		#endregion

		public void Change(ILanguage language)
		{
			_language = language;
		}
	}
}