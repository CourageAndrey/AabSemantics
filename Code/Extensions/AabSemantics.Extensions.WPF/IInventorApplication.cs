using System;
using System.Collections.Generic;

namespace AabSemantics.Extensions.WPF
{
	public interface IInventorApplication
	{
		String StartupPath
		{ get; }

		ILanguage CurrentLanguage
		{ get; set; }

		ICollection<ILanguage> Languages
		{ get; }

		InventorConfiguration Configuration
		{ get; }

		IMainWindow MainForm
		{ get; }

		ISemanticNetwork SemanticNetwork
		{ get; set; }
	}
}
