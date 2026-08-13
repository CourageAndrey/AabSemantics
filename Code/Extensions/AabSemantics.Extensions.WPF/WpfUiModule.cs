using System;
using System.Collections.Generic;

using AabSemantics.Localization;

namespace AabSemantics.Extensions.WPF
{
	/// <summary>Module contributing the WPF interface's own localized captions.</summary>
	public class WpfUiModule : ExtensionModule
	{
		/// <summary>Name the module is registered under.</summary>
		public const String ModuleName = "UI.WPF";

		/// <summary>Creates the module.</summary>
		public WpfUiModule()
			: base(ModuleName)
		{ }

		/// <summary>Adds the module's English texts to the built-in default language.</summary>
		protected override void RegisterLanguage()
		{
			Language.Default.Extensions.Add(Localization.WpfUiModule.CreateDefault());
		}

		/// <summary>Declares the module's string bundle type for the XML serializer.</summary>
		/// <returns>A single entry mapping the module name to its bundle type.</returns>
		public override IDictionary<String, Type> GetLanguageExtensions()
		{
			return new Dictionary<String, Type>
			{
				{ nameof(Localization.WpfUiModule), typeof(Localization.WpfUiModule) }
			};
		}
	}
}
