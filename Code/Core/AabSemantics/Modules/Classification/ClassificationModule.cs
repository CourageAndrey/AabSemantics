﻿using System;
using System.Collections.Generic;

using AabSemantics.Localization;
using AabSemantics.Metadata;
using AabSemantics.Modules.Classification.Localization;
using AabSemantics.Modules.Classification.Questions;
using AabSemantics.Modules.Classification.Statements;

namespace AabSemantics.Modules.Classification
{
	public class ClassificationModule : ExtensionModule
	{
		public const String ModuleName = "System.Classification";

		public ClassificationModule()
			: base(ModuleName, new[] { Boolean.BooleanModule.ModuleName })
		{ }

		protected override void RegisterLanguage()
		{
			Language.Default.Extensions.Add(LanguageClassificationModule.CreateDefault());
		}

		protected override void RegisterStatements()
		{
			Repositories.RegisterStatement<IsStatement, ILanguageClassificationModule, Localization.ILanguageStatements, ILanguageStatementsPart>(
					part => part.Classification,
					statement => new Dictionary<String, IKnowledge>
					{
						{ Strings.ParamParent, statement.Ancestor },
						{ Strings.ParamChild, statement.Descendant },
					},
					checkCyclicParents)
				.SerializeToXml(statement => new Xml.IsStatement(statement))
				.SerializeToJson(statement => new Json.IsStatement(statement));
			Repositories.RegisterCustomStatement<IsStatement, ILanguageClassificationModule, Localization.ILanguageStatements, ILanguageStatementsPart>(
				new List<String> { Strings.ParamParent, Strings.ParamChild },
				part => part.Classification);
		}

		protected override void RegisterQuestions()
		{
			Repositories.RegisterQuestion<EnumerateAncestorsQuestion>(language => language.GetQuestionsExtension<ILanguageClassificationModule, Localization.ILanguageQuestions>().Names.EnumerateAncestorsQuestion)
				.SerializeToXml(question => new Xml.EnumerateAncestorsQuestion(question))
				.SerializeToJson(question => new Json.EnumerateAncestorsQuestion(question));
			Repositories.RegisterQuestion<EnumerateDescendantsQuestion>(language => language.GetQuestionsExtension<ILanguageClassificationModule, Localization.ILanguageQuestions>().Names.EnumerateDescendantsQuestion)
				.SerializeToXml(question => new Xml.EnumerateDescendantsQuestion(question))
				.SerializeToJson(question => new Json.EnumerateDescendantsQuestion(question));
			Repositories.RegisterQuestion<IsQuestion>(language => language.GetQuestionsExtension<ILanguageClassificationModule, Localization.ILanguageQuestions>().Names.IsQuestion)
				.SerializeToXml(question => new Xml.IsQuestion(question))
				.SerializeToJson(question => new Json.IsQuestion(question));
		}

		public override IDictionary<String, Type> GetLanguageExtensions()
		{
			return new Dictionary<String, Type>
			{
				{ nameof(ClassificationModule), typeof(LanguageClassificationModule) }
			};
		}

		private static void checkCyclicParents(
			ISemanticNetwork semanticNetwork,
			ITextContainer result,
			ICollection<IsStatement> statements)
		{
			foreach (var classification in statements)
			{
				if (!classification.CheckCyclic(statements))
				{
					result.Append(
						language => language.GetStatementsExtension<ILanguageClassificationModule, Localization.ILanguageStatements>().Consistency.ErrorCyclic,
						new Dictionary<String, IKnowledge> { { Strings.ParamStatement, classification } });
				}
			}
		}
	}
}
