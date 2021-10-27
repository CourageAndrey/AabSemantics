using System;
using System.Collections.Generic;

using Inventor.Semantics.Attributes;
using Inventor.Semantics.Concepts;
using Inventor.Semantics.Localization;
using Inventor.Semantics.Localization.Modules;
using Inventor.Semantics.Metadata;
using Inventor.Semantics.Questions;

namespace Inventor.Semantics.Modules
{
	public class BooleanModule : ExtensionModule
	{
		public const String ModuleName = "System.Boolean";

		public BooleanModule()
			: base(ModuleName)
		{ }

		protected override void Attach(ISemanticNetwork semanticNetwork)
		{
			foreach (var boolean in LogicalValues.All)
			{
				semanticNetwork.Concepts.Add(boolean);
			}
		}

		protected override void RegisterLanguage()
		{
			Language.Default.Extensions.Add(LanguageBooleanModule.CreateDefault());
		}

		protected override void RegisterAttributes()
		{
			Repositories.RegisterAttribute(IsValueAttribute.Value, language => language.GetExtension<ILanguageBooleanModule>().Attributes.IsValue, new Xml.IsValueAttribute());
			Repositories.RegisterAttribute(IsBooleanAttribute.Value, language => language.GetExtension<ILanguageBooleanModule>().Attributes.IsBoolean, new Xml.IsBooleanAttribute());
		}

		protected override void RegisterConcepts()
		{
			Xml.ConceptIdResolver.RegisterEnumType(typeof(LogicalValues));
		}

		protected override void RegisterQuestions()
		{
			Repositories.RegisterQuestion<CheckStatementQuestion>(language => language.GetExtension<ILanguageBooleanModule>().Questions.Names.CheckStatementQuestion);
		}

		public override IDictionary<String, Type> GetLanguageExtensions()
		{
			return new Dictionary<String, Type>
			{
				{ nameof(BooleanModule), typeof(LanguageBooleanModule) }
			};
		}
	}
}
