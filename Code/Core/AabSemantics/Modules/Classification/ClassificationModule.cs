using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using AabSemantics.Localization;
using AabSemantics.Metadata;
using AabSemantics.Modules.Classification.Localization;
using AabSemantics.Modules.Classification.Questions;
using AabSemantics.Modules.Classification.Statements;

namespace AabSemantics.Modules.Classification
{
	/// <summary>
	/// Built-in module supplying the "is a" relation and the questions over it. Its hierarchy is
	/// what the engine's transitive inference walks, so most other modules build on it.
	/// Depends on the boolean module.
	/// </summary>
	public class ClassificationModule : ExtensionModule
	{
		/// <summary>Name the module is registered under.</summary>
		public const String ModuleName = "System.Classification";

		/// <summary>Creates the module, declaring its dependency on the boolean module.</summary>
		public ClassificationModule()
			: base(ModuleName, new[] { Boolean.BooleanModule.ModuleName })
		{ }

		/// <summary>Adds the module's English texts to the built-in default language.</summary>
		protected override void RegisterLanguage()
		{
			Language.Default.Extensions.Add(LanguageClassificationModule.CreateDefault());
		}

		/// <summary>
		/// Registers the "is a" statement with a cycle check, and the same relation again as a
		/// custom statement kind so it can also be used without the compiled type.
		/// </summary>
		protected override void RegisterStatements()
		{
			Repositories.RegisterStatement<IsStatement, ILanguageClassificationModule, Localization.ILanguageStatements, ILanguageStatementsPart>(
					part => part.Classification,
					statement => new Dictionary<String, IKnowledge>
					{
						{ Strings.ParamParent, statement.Ancestor },
						{ Strings.ParamChild, statement.Descendant },
					},
					CheckCyclicParentsAsync)
				.SerializeToXml(statement => new Xml.IsStatement(statement))
				.SerializeToJson(statement => new Json.IsStatement(statement));
			Repositories.RegisterCustomStatement<IsStatement, ILanguageClassificationModule, Localization.ILanguageStatements, ILanguageStatementsPart>(
				new List<String> { Strings.ParamParent, Strings.ParamChild },
				part => part.Classification);
		}

		/// <summary>Registers the ancestor, descendant and "is a" questions with their persistence.</summary>
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

		/// <summary>Declares the module's string bundle type for the XML serializer.</summary>
		/// <returns>A single entry mapping the module name to its bundle type.</returns>
		public override IDictionary<String, Type> GetLanguageExtensions()
		{
			return new Dictionary<String, Type>
			{
				{ nameof(ClassificationModule), typeof(LanguageClassificationModule) }
			};
		}

		private static async Task CheckCyclicParentsAsync(
			ISemanticNetwork semanticNetwork,
			ITextContainer result,
			ICollection<IsStatement> statements)
		{
			foreach (var classification in statements)
			{
				if (! await classification.CheckCyclicAsync(statements))
				{
					result.Append(
						language => language.GetStatementsExtension<ILanguageClassificationModule, Localization.ILanguageStatements>().Consistency.ErrorCyclic,
						new Dictionary<String, IKnowledge> { { Strings.ParamStatement, classification } });
				}
			}
		}
	}
}
