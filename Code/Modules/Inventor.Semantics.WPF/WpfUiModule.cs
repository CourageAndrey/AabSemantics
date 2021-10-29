﻿using System;
using System.Collections.Generic;

using Inventor.Semantics.Localization;

namespace Inventor.Semantics.WPF
{
	public class WpfUiModule : ExtensionModule
	{
		public const String ModuleName = "UI.WPF";

		public WpfUiModule()
			: base(ModuleName)
		{ }

		protected override void RegisterLanguage()
		{
			Language.Default.Extensions.Add(Localization.WpfUiModule.CreateDefault());
		}

		public override IDictionary<String, Type> GetLanguageExtensions()
		{
			return new Dictionary<String, Type>
			{
				{ nameof(Localization.WpfUiModule), typeof(Localization.WpfUiModule) }
			};
		}
	}
}
