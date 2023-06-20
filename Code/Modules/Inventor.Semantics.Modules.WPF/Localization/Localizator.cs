using System;
using System.Collections.Generic;

using Inventor.Semantics.Localization;

namespace Inventor.Semantics.WPF.Localization
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
		{ get { return _language?.GetExtension<IWpfUiModule>().Common; } }

		public ILanguageErrors Errors
		{ get { return _language?.GetExtension<IWpfUiModule>().Errors; } }

		public ILanguageAttributes Attributes
		{ get { return _language?.Attributes; } }

		public ILanguageStatements Statements
		{ get { return _language?.Statements; } }

		public ILanguageQuestions Questions
		{ get { return _language?.Questions; } }

		public ICollection<LanguageExtension> Extensions
		{ get { return _language?.Extensions; } }

		public ILanguageUi Ui
		{ get { return _language?.GetExtension<IWpfUiModule>().Ui; } }

		public ILanguageMisc Misc
		{ get { return _language?.GetExtension<IWpfUiModule>().Misc; } }

		#endregion

		public void Change(ILanguage language)
		{
			_language = language;
		}
	}
}