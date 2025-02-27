using System;
using System.Collections.Generic;

using AabSemantics.Metadata;
using AabSemantics.Modules.Mathematics.Attributes;
using AabSemantics.Modules.Mathematics.Concepts;
using AabSemantics.Modules.Mathematics.Localization;
using AabSemantics.Modules.Mathematics.Questions;
using AabSemantics.Modules.Mathematics.Statements;
using AabSemantics.Serialization;

namespace AabSemantics.Modules.Mathematics
{
	public class MathematicsModule : ExtensionModule
	{
		public const String ModuleName = "System.Mathematics";

		public MathematicsModule()
			: base(ModuleName)
		{ }

		protected override void Attach(ISemanticNetwork semanticNetwork)
		{
			foreach (var sign in ComparisonSigns.All)
			{
				semanticNetwork.Concepts.Add(sign);
			}
		}

		protected override void RegisterLanguage()
		{
			AabSemantics.Localization.Language.Default.Extensions.Add(LanguageMathematicsModule.CreateDefault());
		}

		protected override void RegisterAttributes()
		{
			Repositories.RegisterAttribute(IsComparisonSignAttribute.Value, language => language.GetAttributesExtension<ILanguageMathematicsModule, ILanguageAttributes>().IsComparisonSign)
				.SerializeToXml(new Xml.IsComparisonSignAttribute())
				.SerializeToJson(new Xml.IsComparisonSignAttribute());
		}

		protected override void RegisterConcepts()
		{
			ConceptIdResolver.RegisterEnumType(typeof(ComparisonSigns));
		}

		protected override void RegisterStatements()
		{
			Repositories.RegisterStatement<ComparisonStatement>(
					language => language.GetStatementsExtension<ILanguageMathematicsModule, ILanguageStatements>().Names.Comparison,
					language => language.GetStatementsExtension<ILanguageMathematicsModule, ILanguageStatements>().TrueFormatStrings.Comparison,
					language => language.GetStatementsExtension<ILanguageMathematicsModule, ILanguageStatements>().FalseFormatStrings.Comparison,
					language => language.GetStatementsExtension<ILanguageMathematicsModule, ILanguageStatements>().QuestionFormatStrings.Comparison,
					statement => new Dictionary<String, IKnowledge>
					{
						{ Strings.ParamLeftValue, statement.LeftValue },
						{ Strings.ParamRightValue, statement.RightValue },
						{ Strings.ParamComparisonSign, statement.ComparisonSign },
					},
					checkComparisonValueSystems)
				.SerializeToXml(statement => new Xml.ComparisonStatement(statement))
				.SerializeToJson(statement => new Json.ComparisonStatement(statement));
		}

		protected override void RegisterQuestions()
		{
			Repositories.RegisterQuestion<ComparisonQuestion>(language => language.GetQuestionsExtension<ILanguageMathematicsModule, ILanguageQuestions>().Names.ComparisonQuestion)
				.SerializeToXml(question => new Xml.ComparisonQuestion(question))
				.SerializeToJson(question => new Json.ComparisonQuestion(question));
		}

		public override IDictionary<String, Type> GetLanguageExtensions()
		{
			return new Dictionary<String, Type>
			{
				{ nameof(MathematicsModule), typeof(LanguageMathematicsModule) }
			};
		}

		private static void checkComparisonValueSystems(
			ISemanticNetwork semanticNetwork,
			ITextContainer result,
			ICollection<ComparisonStatement> statements)
		{
			foreach (var contradiction in statements.CheckForContradictions())
			{
				result
					.Append(
						language => language.GetStatementsExtension<ILanguageMathematicsModule, ILanguageStatements>().Consistency.ErrorComparisonContradiction,
						new Dictionary<String, IKnowledge>
						{
							{ Strings.ParamLeftValue, contradiction.Value1 },
							{ Strings.ParamRightValue, contradiction.Value2 },
						})
					.Append(contradiction.Signs.EnumerateOneLine());
			}
		}
	}
}
