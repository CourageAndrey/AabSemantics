using System;
using System.Collections.Generic;

namespace AabSemantics.Extensions.WPF
{
	/// <summary>The hosting application, as seen by the UI extension.</summary>
	public interface IInventorApplication
	{
		/// <summary>Directory the application was started from; language files are resolved against it.</summary>
		String StartupPath
		{ get; }

		/// <summary>Language the interface is currently shown in.</summary>
		ILanguage CurrentLanguage
		{ get; set; }

		/// <summary>Languages available to choose from.</summary>
		ICollection<ILanguage> Languages
		{ get; }

		/// <summary>Persisted user settings.</summary>
		InventorConfiguration Configuration
		{ get; }

		/// <summary>The application's main window.</summary>
		IMainWindow MainForm
		{ get; }

		/// <summary>The knowledge base currently open.</summary>
		ISemanticNetwork SemanticNetwork
		{ get; set; }
	}
}
