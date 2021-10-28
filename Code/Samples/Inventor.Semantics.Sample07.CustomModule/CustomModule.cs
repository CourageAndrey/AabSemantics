using System;
using System.Collections.Generic;

using Inventor.Semantics;
using Inventor.Semantics.Concepts;
using Inventor.Semantics.Localization;
using Inventor.Semantics.Metadata;
using Inventor.Semantics.Modules;
using Inventor.Semantics.Xml;
using Samples._07.CustomModule.Localization;

namespace Samples._07.CustomModule
{
	internal class CustomModule : ExtensionModule
	{
		public const String ModuleName = "Sample.Custom";

		public CustomModule()
			: base(ModuleName, new[] { BooleanModule.ModuleName })
		{ }

		protected override void Attach(ISemanticNetwork semanticNetwork)
		{
			semanticNetwork.Concepts.Add(CustomConcepts.Custom);
		}

		public override IDictionary<string, Type> GetLanguageExtensions()
		{
			return new Dictionary<String, Type>
			{
				{ nameof(CustomModule), typeof(LanguageCustomModule) }
			};
		}

		protected override void RegisterLanguage()
		{
			Language.Default.Extensions.Add(LanguageCustomModule.CreateDefault());
		}

		protected override void RegisterAttributes()
		{
			Repositories.RegisterAttribute(
				CustomAttribute.Value,
				language => language.GetExtension<ILanguageCustomModule>().Attributes.Custom,
				new Xml.CustomAttribute());
		}

		protected override void RegisterConcepts()
		{
			ConceptIdResolver.RegisterEnumType(typeof(CustomConcepts));
		}

		protected override void RegisterStatements()
		{
			Repositories.RegisterStatement<CustomStatement>(
				language => language.GetExtension<ILanguageCustomModule>().Statements.Names.Custom,
				statement => new Xml.CustomStatement(statement),
				typeof(Xml.CustomStatement),
				checkSelfRelations);
		}

		protected override void RegisterQuestions()
		{
			Repositories.RegisterQuestion<CustomQuestion>(language => language.GetExtension<ILanguageCustomModule>().Questions.Names.Custom);
		}

		private static void checkSelfRelations(
			ICollection<CustomStatement> statements,
			ITextContainer result,
			ISemanticNetwork semanticNetwork)
		{
			foreach (var statement in statements)
			{
				if (statement.Concept1 == statement.Concept2)
				{
					result.Append(
						language => language.GetExtension<ILanguageCustomModule>().Statements.Consistency.SelfReference,
						new Dictionary<String, IKnowledge> { { CustomStatement.ParamConcept1, statement.Concept1 } });
				}
			}
		}
	}
}
