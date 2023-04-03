﻿using System;
using System.Collections.Generic;

using Inventor.Semantics.Metadata;
using Inventor.Semantics.Mathematics.Attributes;
using Inventor.Semantics.Mathematics.Concepts;
using Inventor.Semantics.Mathematics.Localization;
using Inventor.Semantics.Mathematics.Questions;
using Inventor.Semantics.Mathematics.Statements;
using Inventor.Semantics.Serialization;

namespace Inventor.Semantics.Mathematics
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
			Repositories.RegisterAttribute(IsComparisonSignAttribute.Value, language => language.GetExtension<ILanguageMathematicsModule>().Attributes.IsComparisonSign)
				.SerializeToXml(new Xml.IsComparisonSignAttribute())
				.SerializeToJson(new Xml.IsComparisonSignAttribute());
		}

		protected override void RegisterConcepts()
		{
			ConceptIdResolver.RegisterEnumType(typeof(ComparisonSigns));
		}

		protected override void RegisterStatements()
		{
			Repositories.RegisterStatement<ComparisonStatement>(language => language.GetExtension<ILanguageMathematicsModule>().Statements.Names.Comparison, checkComparisonValueSystems)
				.SerializeToXml(statement => new Xml.ComparisonStatement(statement))
				.SerializeToJson(statement => new Json.ComparisonStatement(statement));
		}

		protected override void RegisterQuestions()
		{
			Repositories.RegisterQuestion<ComparisonQuestion>(language => language.GetExtension<ILanguageMathematicsModule>().Questions.Names.ComparisonQuestion)
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
