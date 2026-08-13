using System;
using System.Collections.Generic;

using AabSemantics.Localization;

namespace AabSemantics.Extensions.WPF.Localization
{
	/// <summary>
	/// A mutable <see cref="ILanguage"/> the UI binds to. Switching languages replaces the wrapped
	/// language in place, so bound controls update without being rebuilt.
	/// </summary>
	public class Localizator : ILanguage
	{
		private ILanguage _language;

		/// <summary>Creates a localizator over the built-in default language.</summary>
		public Localizator()
			: this(null)
		{ }

		/// <summary>Creates a localizator over a language.</summary>
		/// <param name="language">Language to start with.</param>
		public Localizator(ILanguage language)
		{
			_language = language;
		}

		#region Properties

		/// <summary>Display name of the language, written in that language itself.</summary>
		public String Name
		{ get { return _language?.Name; } }

		/// <summary>Culture identifier of the language.</summary>
		public String Culture
		{ get { return _language?.Culture; } }

		/// <summary>Captions shared by every dialog.</summary>
		public ILanguageCommon Common
		{ get { return _language?.GetExtension<IWpfUiModule>().Common; } }

		/// <summary>Wordings of the error dialog.</summary>
		public ILanguageErrors Errors
		{ get { return _language?.GetExtension<IWpfUiModule>().Errors; } }

		/// <summary>Attribute names of the wrapped language.</summary>
		public ILanguageAttributes Attributes
		{ get { return _language?.Attributes; } }

		/// <summary>Child node grouping the statements.</summary>
		public ILanguageStatements Statements
		{ get { return _language?.Statements; } }

		/// <summary>Question wordings of the wrapped language.</summary>
		public ILanguageQuestions Questions
		{ get { return _language?.Questions; } }

		/// <summary>Per-module string bundles of the wrapped language.</summary>
		public ICollection<LanguageExtension> Extensions
		{ get { return _language?.Extensions; } }

		/// <summary>Captions of the wizard-style dialogs.</summary>
		public ILanguageUi Ui
		{ get { return _language?.GetExtension<IWpfUiModule>().Ui; } }

		/// <summary>Captions that do not belong to any single dialog.</summary>
		public ILanguageMisc Misc
		{ get { return _language?.GetExtension<IWpfUiModule>().Misc; } }

		#endregion

		/// <summary>Switches to another language, keeping every existing binding valid.</summary>
		/// <param name="language">Language to switch to.</param>
		public void Change(ILanguage language)
		{
			_language = language;
		}
	}
}