using System;
using System.Collections.Generic;

using Inventor.Semantics;
using Inventor.Semantics.Metadata;
using Inventor.Mathematics.Attributes;
using Inventor.Mathematics.Concepts;
using Inventor.Mathematics.Localization;
using Inventor.Mathematics.Questions;
using Inventor.Mathematics.Statements;

namespace Inventor.Mathematics
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
			Semantics.Localization.Language.Default.Extensions.Add(LanguageMathematicsModule.CreateDefault());
		}

		protected override void RegisterAttributes()
		{
			Repositories.RegisterAttribute(IsComparisonSignAttribute.Value, language => language.GetExtension<ILanguageMathematicsModule>().Attributes.IsComparisonSign, new Xml.IsComparisonSignAttribute());
		}

		protected override void RegisterConcepts()
		{
			Semantics.Xml.ConceptIdResolver.RegisterEnumType(typeof(ComparisonSigns));
		}

		protected override void RegisterStatements()
		{
			Repositories.RegisterStatement<ComparisonStatement>(
				language => language.GetExtension<ILanguageMathematicsModule>().Statements.Names.Comparison,
				statement => new Xml.ComparisonStatement(statement),
				typeof(Xml.ComparisonStatement),
				checkComparisonValueSystems);
		}

		protected override void RegisterQuestions()
		{
			Repositories.RegisterQuestion<ComparisonQuestion>(language => language.GetExtension<ILanguageMathematicsModule>().Questions.Names.ComparisonQuestion);
		}

		public override IDictionary<String, Type> GetLanguageExtensions()
		{
			return new Dictionary<String, Type>
			{
				{ nameof(MathematicsModule), typeof(LanguageMathematicsModule) }
			};
		}

		private static void checkComparisonValueSystems(
			ICollection<ComparisonStatement> statements,
			ITextContainer result,
			ISemanticNetwork semanticNetwork)
		{
			foreach (var contradiction in statements.CheckForContradictions())
			{
				result
					.Append(
						language => language.GetExtension<ILanguageMathematicsModule>().Statements.Consistency.ErrorComparisonContradiction,
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
