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
			Repositories.RegisterStatement<IsStatement>(
				language => language.GetExtension<ILanguageClassificationModule>().Statements.Names.Clasification,
				statement => new Xml.IsStatement(statement),
				typeof(Xml.IsStatement),
				checkCyclicParents);
		}

		protected override void RegisterQuestions()
		{
			Repositories.RegisterQuestion<EnumerateAncestorsQuestion>(language => language.GetExtension<ILanguageClassificationModule>().Questions.Names.EnumerateAncestorsQuestion);
			Repositories.RegisterQuestion<EnumerateDescendantsQuestion>(language => language.GetExtension<ILanguageClassificationModule>().Questions.Names.EnumerateDescendantsQuestion);
			Repositories.RegisterQuestion<IsQuestion>(language => language.GetExtension<ILanguageClassificationModule>().Questions.Names.IsQuestion);
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
