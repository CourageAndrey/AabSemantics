using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using AabSemantics.Metadata;
using AabSemantics.Modules.Mathematics.Attributes;
using AabSemantics.Modules.Mathematics.Concepts;
using AabSemantics.Modules.Mathematics.Localization;
using AabSemantics.Modules.Mathematics.Questions;
using AabSemantics.Modules.Mathematics.Statements;
using AabSemantics.Serialization;

namespace AabSemantics.Modules.Mathematics
{
	/// <summary>
	/// Built-in module supplying the six comparison signs, the comparison statement with its
	/// contradiction check, and the comparison question. Depends on the boolean module.
	/// </summary>
	public class MathematicsModule : ExtensionModule
	{
		/// <summary>Name the module is registered under.</summary>
		public const String ModuleName = "System.Mathematics";

		/// <summary>Creates the module, declaring its dependency on the boolean module.</summary>
		public MathematicsModule()
			: base(ModuleName)
		{ }

		/// <summary>Adds the comparison sign concepts to the network.</summary>
		/// <param name="semanticNetwork">Network being extended.</param>
		protected override void Attach(ISemanticNetwork semanticNetwork)
		{
			foreach (var sign in ComparisonSigns.All)
			{
				semanticNetwork.Concepts.Add(sign);
			}
		}

		/// <summary>Adds the module's English texts to the built-in default language.</summary>
		protected override void RegisterLanguage()
		{
			AabSemantics.Localization.Language.Default.Extensions.Add(LanguageMathematicsModule.CreateDefault());
		}

		/// <summary>Registers the "is a comparison sign" attribute.</summary>
		protected override void RegisterAttributes()
		{
			Repositories.RegisterAttribute(IsComparisonSignAttribute.Value, language => language.GetAttributesExtension<ILanguageMathematicsModule, ILanguageAttributes>().IsComparisonSign)
				.SerializeToXml(new Xml.IsComparisonSignAttribute())
				.SerializeToJson(new Xml.IsComparisonSignAttribute());
		}

		/// <summary>Makes the comparison sign concepts resolvable by identifier during deserialization.</summary>
		protected override void RegisterConcepts()
		{
			ConceptIdResolver.RegisterEnumType(typeof(ComparisonSigns));
		}

		/// <summary>Registers the comparison statement, its contradiction check and its custom-statement form.</summary>
		protected override void RegisterStatements()
		{
			Repositories.RegisterStatement<ComparisonStatement, ILanguageMathematicsModule, ILanguageStatements, ILanguageStatementsPart>(
					language => language.Comparison,
					statement => new Dictionary<String, IKnowledge>
					{
						{ Strings.ParamLeftValue, statement.LeftValue },
						{ Strings.ParamRightValue, statement.RightValue },
						{ Strings.ParamComparisonSign, statement.ComparisonSign },
					},
					CheckComparisonValueSystemsAsync)
				.SerializeToXml(statement => new Xml.ComparisonStatement(statement))
				.SerializeToJson(statement => new Json.ComparisonStatement(statement));
			Repositories.RegisterCustomStatement<ComparisonStatement, ILanguageMathematicsModule, ILanguageStatements, ILanguageStatementsPart>(
				new List<String> { nameof(ComparisonStatement.LeftValue), nameof(ComparisonStatement.RightValue), nameof(ComparisonStatement.ComparisonSign) },
				language => language.Comparison);
		}

		/// <summary>Registers the comparison question and its persistence.</summary>
		protected override void RegisterQuestions()
		{
			Repositories.RegisterQuestion<ComparisonQuestion>(language => language.GetQuestionsExtension<ILanguageMathematicsModule, ILanguageQuestions>().Names.ComparisonQuestion)
				.SerializeToXml(question => new Xml.ComparisonQuestion(question))
				.SerializeToJson(question => new Json.ComparisonQuestion(question));
		}

		/// <summary>Declares the module's string bundle type for the XML serializer.</summary>
		/// <returns>A single entry mapping the module name to its bundle type.</returns>
		public override IDictionary<String, Type> GetLanguageExtensions()
		{
			return new Dictionary<String, Type>
			{
				{ nameof(MathematicsModule), typeof(LanguageMathematicsModule) }
			};
		}

		private static async Task CheckComparisonValueSystemsAsync(
			ISemanticNetwork semanticNetwork,
			ITextContainer result,
			ICollection<ComparisonStatement> statements,
			CancellationToken cancellationToken)
		{
			foreach (var contradiction in await statements.CheckForContradictionsAsync(cancellationToken).ConfigureAwait(false))
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
