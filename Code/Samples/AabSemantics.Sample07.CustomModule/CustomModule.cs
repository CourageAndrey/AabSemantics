using System;
using System.Collections.Generic;

using AabSemantics.Localization;
using AabSemantics.Metadata;
using AabSemantics.Modules.Boolean;
using AabSemantics.Serialization;
using AabSemantics.Sample07.CustomModule.Localization;

namespace AabSemantics.Sample07.CustomModule
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

		protected override void RegisterAnswers()
		{
			Repositories.RegisterAnswer<CustomAnswer>()
				.SerializeToXml((answer, language) => new Xml.CustomAnswer())
				.SerializeToJson((answer, language) => new Json.CustomAnswer());
		}

		protected override void RegisterAttributes()
		{
			Repositories.RegisterAttribute(CustomAttribute.Value, language => language.GetAttributesExtension<ILanguageCustomModule, Localization.ILanguageAttributes>().Custom)
				.SerializeToXml(new Xml.CustomAttribute())
				.SerializeToJson(new Xml.CustomAttribute());
		}

		protected override void RegisterConcepts()
		{
			ConceptIdResolver.RegisterEnumType(typeof(CustomConcepts));
		}

		protected override void RegisterStatements()
		{
			Repositories.RegisterStatement<CustomStatement>(
					language => language.GetStatementsExtension<ILanguageCustomModule, Localization.ILanguageStatements>().Names.Custom,
					language => language.GetStatementsExtension<ILanguageCustomModule, Localization.ILanguageStatements>().TrueFormatStrings.Custom,
					language => language.GetStatementsExtension<ILanguageCustomModule, Localization.ILanguageStatements>().FalseFormatStrings.Custom,
					language => language.GetStatementsExtension<ILanguageCustomModule, Localization.ILanguageStatements>().QuestionFormatStrings.Custom,
					statement => new Dictionary<string, IKnowledge>
					{
						{ CustomStatement.ParamConcept1, statement.Concept1 },
						{ CustomStatement.ParamConcept2, statement.Concept2 },
					},
					checkSelfRelations)
				.SerializeToXml(statement => new Xml.CustomStatement(statement))
				.SerializeToJson(statement => new Json.CustomStatement(statement));
		}

		protected override void RegisterQuestions()
		{
			Repositories.RegisterQuestion<CustomQuestion>(language => language.GetQuestionsExtension<ILanguageCustomModule, Localization.ILanguageQuestions>().Names.Custom)
				.SerializeToXml(question => new Xml.CustomQuestion(question))
				.SerializeToJson(question => new Json.CustomQuestion(question));
		}

		private static void checkSelfRelations(
			ISemanticNetwork semanticNetwork,
			ITextContainer result,
			ICollection<CustomStatement> statements)
		{
			foreach (var statement in statements)
			{
				if (statement.Concept1 == statement.Concept2)
				{
					result.Append(
						language => language.GetStatementsExtension<ILanguageCustomModule, Localization.ILanguageStatements>().Consistency.SelfReference,
						new Dictionary<String, IKnowledge> { { CustomStatement.ParamConcept1, statement.Concept1 } });
				}
			}
		}
	}
}
