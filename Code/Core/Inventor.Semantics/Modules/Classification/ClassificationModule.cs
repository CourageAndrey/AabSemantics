using System;
using System.Collections.Generic;

using Inventor.Semantics.Metadata;
using Inventor.Semantics.Modules.Classification.Localization;
using Inventor.Semantics.Modules.Classification.Questions;
using Inventor.Semantics.Modules.Classification.Statements;

namespace Inventor.Semantics.Modules.Classification
{
	public class ClassificationModule : ExtensionModule
	{
		public const String ModuleName = "System.Classification";

		public ClassificationModule()
			: base(ModuleName, new[] { Boolean.BooleanModule.ModuleName })
		{ }

		protected override void RegisterLanguage()
		{
			Semantics.Localization.Language.Default.Extensions.Add(LanguageClassificationModule.CreateDefault());
		}

		protected override void RegisterStatements()
		{
			Repositories.RegisterStatement<IsStatement, Xml.IsStatement>(
				language => language.GetExtension<ILanguageClassificationModule>().Statements.Names.Clasification,
				statement => new Xml.IsStatement(statement),
				checkCyclicParents);
		}

		protected override void RegisterQuestions()
		{
			Repositories.RegisterQuestion<EnumerateAncestorsQuestion, Xml.EnumerateAncestorsQuestion>(
				language => language.GetExtension<ILanguageClassificationModule>().Questions.Names.EnumerateAncestorsQuestion,
				question => new Xml.EnumerateAncestorsQuestion(question));
			Repositories.RegisterQuestion<EnumerateDescendantsQuestion, Xml.EnumerateDescendantsQuestion>(
				language => language.GetExtension<ILanguageClassificationModule>().Questions.Names.EnumerateDescendantsQuestion,
				question => new Xml.EnumerateDescendantsQuestion(question));
			Repositories.RegisterQuestion<IsQuestion, Xml.IsQuestion>(
				language => language.GetExtension<ILanguageClassificationModule>().Questions.Names.IsQuestion,
				question => new Xml.IsQuestion(question));
		}

		public override IDictionary<String, Type> GetLanguageExtensions()
		{
			return new Dictionary<String, Type>
			{
				{ nameof(ClassificationModule), typeof(LanguageClassificationModule) }
			};
		}

		private static void checkCyclicParents(
			ICollection<IsStatement> statements,
			ITextContainer result,
			ISemanticNetwork semanticNetwork)
		{
			foreach (var clasification in statements)
			{
				if (!clasification.CheckCyclic(statements))
				{
					result.Append(
						language => language.GetExtension<ILanguageClassificationModule>().Statements.Consistency.ErrorCyclic,
						new Dictionary<String, IKnowledge> { { Semantics.Localization.Strings.ParamStatement, clasification } });
				}
			}
		}
	}
}
