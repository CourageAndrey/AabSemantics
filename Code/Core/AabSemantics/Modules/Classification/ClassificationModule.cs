using System;
using System.Collections.Generic;

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
			AabSemantics.Localization.Language.Default.Extensions.Add(LanguageClassificationModule.CreateDefault());
		}

		protected override void RegisterStatements()
		{
			Repositories.RegisterStatement<IsStatement>(language => language.GetExtension<ILanguageClassificationModule>().Statements.Names.Clasification, checkCyclicParents)
				.SerializeToXml(statement => new Xml.IsStatement(statement))
				.SerializeToJson(statement => new Json.IsStatement(statement));
		}

		protected override void RegisterQuestions()
		{
			Repositories.RegisterQuestion<EnumerateAncestorsQuestion>(language => language.GetExtension<ILanguageClassificationModule>().Questions.Names.EnumerateAncestorsQuestion)
				.SerializeToXml(question => new Xml.EnumerateAncestorsQuestion(question))
				.SerializeToJson(question => new Json.EnumerateAncestorsQuestion(question));
			Repositories.RegisterQuestion<EnumerateDescendantsQuestion>(language => language.GetExtension<ILanguageClassificationModule>().Questions.Names.EnumerateDescendantsQuestion)
				.SerializeToXml(question => new Xml.EnumerateDescendantsQuestion(question))
				.SerializeToJson(question => new Json.EnumerateDescendantsQuestion(question));
			Repositories.RegisterQuestion<IsQuestion>(language => language.GetExtension<ILanguageClassificationModule>().Questions.Names.IsQuestion)
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
			foreach (var clasification in statements)
			{
				if (!clasification.CheckCyclic(statements))
				{
					result.Append(
						language => language.GetExtension<ILanguageClassificationModule>().Statements.Consistency.ErrorCyclic,
						new Dictionary<String, IKnowledge> { { AabSemantics.Localization.Strings.ParamStatement, clasification } });
				}
			}
		}
	}
}
