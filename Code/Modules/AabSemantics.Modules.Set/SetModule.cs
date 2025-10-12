using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AabSemantics.Metadata;
using AabSemantics.Modules.Boolean;
using AabSemantics.Modules.Set.Attributes;
using AabSemantics.Modules.Set.Localization;
using AabSemantics.Modules.Set.Questions;
using AabSemantics.Modules.Set.Statements;
using AabSemantics.Utils;

namespace AabSemantics.Modules.Set
{
	public class SetModule : ExtensionModule
	{
		public const String ModuleName = "System.Sets";

		public SetModule()
			: base(ModuleName, new[] { BooleanModule.ModuleName, Classification.ClassificationModule.ModuleName })
		{ }

		protected override void Attach(ISemanticNetwork semanticNetwork)
		{ }

		protected override void RegisterLanguage()
		{
			AabSemantics.Localization.Language.Default.Extensions.Add(LanguageSetModule.CreateDefault());
		}

		protected override void RegisterAttributes()
		{
			Repositories.RegisterAttribute(IsSignAttribute.Value, language => language.GetAttributesExtension<ILanguageSetModule, ILanguageAttributes>().IsSign)
				.SerializeToXml(new Xml.IsSignAttribute())
				.SerializeToJson(new Xml.IsSignAttribute());
		}

		protected override void RegisterStatements()
		{
			Repositories.RegisterStatement<HasPartStatement, ILanguageSetModule, ILanguageStatements, ILanguageStatementsPart>(
					language => language.Composition,
					statement => new Dictionary<String, IKnowledge>
					{
						{ AabSemantics.Localization.Strings.ParamParent, statement.Whole },
						{ AabSemantics.Localization.Strings.ParamChild, statement.Part },
					},
					StatementDefinition<HasPartStatement, ILanguageSetModule, ILanguageStatements, ILanguageStatementsPart>.NoConsistencyCheck)
				.SerializeToXml(statement => new Xml.HasPartStatement(statement))
				.SerializeToJson(statement => new Json.HasPartStatement(statement));
			Repositories.RegisterCustomStatement<HasPartStatement, ILanguageSetModule, ILanguageStatements, ILanguageStatementsPart>(
				new List<String> { nameof(HasPartStatement.Part), nameof(HasPartStatement.Whole) },
				language => language.Composition);

			Repositories.RegisterStatement<GroupStatement, ILanguageSetModule, ILanguageStatements, ILanguageStatementsPart>(
					language => language.SubjectArea,
					statement => new Dictionary<String, IKnowledge>
					{
						{ Strings.ParamArea, statement.Area },
						{ AabSemantics.Localization.Strings.ParamConcept, statement.Concept },
					},
					StatementDefinition<GroupStatement, ILanguageSetModule, ILanguageStatements, ILanguageStatementsPart>.NoConsistencyCheck)
				.SerializeToXml(statement => new Xml.GroupStatement(statement))
				.SerializeToJson(statement => new Json.GroupStatement(statement));
			Repositories.RegisterCustomStatement<GroupStatement, ILanguageSetModule, ILanguageStatements, ILanguageStatementsPart>(
				new List<String> { nameof(GroupStatement.Concept), nameof(GroupStatement.Area) },
				language => language.SubjectArea);

			Repositories.RegisterStatement<HasSignStatement, ILanguageSetModule, ILanguageStatements, ILanguageStatementsPart>(
					language => language.HasSign,
					statement => new Dictionary<String, IKnowledge>
					{
						{ AabSemantics.Localization.Strings.ParamConcept, statement.Concept },
						{ Strings.ParamSign, statement.Sign },
					},
					CheckSignDuplicationsAsync)
				.SerializeToXml(statement => new Xml.HasSignStatement(statement))
				.SerializeToJson(statement => new Json.HasSignStatement(statement));
			Repositories.RegisterCustomStatement<HasSignStatement, ILanguageSetModule, ILanguageStatements, ILanguageStatementsPart>(
				new List<String> { nameof(HasSignStatement.Concept), nameof(HasSignStatement.Sign) },
				language => language.HasSign);

			Repositories.RegisterStatement<SignValueStatement, ILanguageSetModule, ILanguageStatements, ILanguageStatementsPart>(
					language => language.SignValue,
					statement => new Dictionary<String, IKnowledge>
					{
						{ AabSemantics.Localization.Strings.ParamConcept, statement.Concept },
						{ Strings.ParamSign, statement.Sign },
						{ Strings.ParamValue, statement.Value },
					},
					CheckSignValuesAsync)
				.SerializeToXml(statement => new Xml.SignValueStatement(statement))
				.SerializeToJson(statement => new Json.SignValueStatement(statement));
			Repositories.RegisterCustomStatement<SignValueStatement, ILanguageSetModule, ILanguageStatements, ILanguageStatementsPart>(
				new List<String> { nameof(SignValueStatement.Concept), nameof(SignValueStatement.Sign), nameof(SignValueStatement.Value) },
				language => language.SignValue);
		}

		protected override void RegisterQuestions()
		{
			Repositories.RegisterQuestion<DescribeSubjectAreaQuestion>(language => language.GetQuestionsExtension<ILanguageSetModule, ILanguageQuestions>().Names.DescribeSubjectAreaQuestion)
				.SerializeToXml(question => new Xml.DescribeSubjectAreaQuestion(question))
				.SerializeToJson(question => new Json.DescribeSubjectAreaQuestion(question));
			Repositories.RegisterQuestion<FindSubjectAreaQuestion>(language => language.GetQuestionsExtension<ILanguageSetModule, ILanguageQuestions>().Names.FindSubjectAreaQuestion)
				.SerializeToXml(question => new Xml.FindSubjectAreaQuestion(question))
				.SerializeToJson(question => new Json.FindSubjectAreaQuestion(question));
			Repositories.RegisterQuestion<IsSubjectAreaQuestion>(language => language.GetQuestionsExtension<ILanguageSetModule, ILanguageQuestions>().Names.IsSubjectAreaQuestion)
				.SerializeToXml(question => new Xml.IsSubjectAreaQuestion(question))
				.SerializeToJson(question => new Json.IsSubjectAreaQuestion(question));

			Repositories.RegisterQuestion<EnumerateContainersQuestion>(language => language.GetQuestionsExtension<ILanguageSetModule, ILanguageQuestions>().Names.EnumerateContainersQuestion)
				.SerializeToXml(question => new Xml.EnumerateContainersQuestion(question))
				.SerializeToJson(question => new Json.EnumerateContainersQuestion(question));
			Repositories.RegisterQuestion<EnumeratePartsQuestion>(language => language.GetQuestionsExtension<ILanguageSetModule, ILanguageQuestions>().Names.EnumeratePartsQuestion)
				.SerializeToXml(question => new Xml.EnumeratePartsQuestion(question))
				.SerializeToJson(question => new Json.EnumeratePartsQuestion(question));
			Repositories.RegisterQuestion<IsPartOfQuestion>(language => language.GetQuestionsExtension<ILanguageSetModule, ILanguageQuestions>().Names.IsPartOfQuestion)
				.SerializeToXml(question => new Xml.IsPartOfQuestion(question))
				.SerializeToJson(question => new Json.IsPartOfQuestion(question));

			Repositories.RegisterQuestion<EnumerateSignsQuestion>(language => language.GetQuestionsExtension<ILanguageSetModule, ILanguageQuestions>().Names.EnumerateSignsQuestion)
				.SerializeToXml(question => new Xml.EnumerateSignsQuestion(question))
				.SerializeToJson(question => new Json.EnumerateSignsQuestion(question));
			Repositories.RegisterQuestion<HasSignQuestion>(language => language.GetQuestionsExtension<ILanguageSetModule, ILanguageQuestions>().Names.HasSignQuestion)
				.SerializeToXml(question => new Xml.HasSignQuestion(question))
				.SerializeToJson(question => new Json.HasSignQuestion(question));
			Repositories.RegisterQuestion<HasSignsQuestion>(language => language.GetQuestionsExtension<ILanguageSetModule, ILanguageQuestions>().Names.HasSignsQuestion)
				.SerializeToXml(question => new Xml.HasSignsQuestion(question))
				.SerializeToJson(question => new Json.HasSignsQuestion(question));
			Repositories.RegisterQuestion<IsSignQuestion>(language => language.GetQuestionsExtension<ILanguageSetModule, ILanguageQuestions>().Names.IsSignQuestion)
				.SerializeToXml(question => new Xml.IsSignQuestion(question))
				.SerializeToJson(question => new Json.IsSignQuestion(question));
			Repositories.RegisterQuestion<IsValueQuestion>(language => language.GetQuestionsExtension<ILanguageSetModule, ILanguageQuestions>().Names.IsValueQuestion)
				.SerializeToXml(question => new Xml.IsValueQuestion(question))
				.SerializeToJson(question => new Json.IsValueQuestion(question));
			Repositories.RegisterQuestion<SignValueQuestion>(language => language.GetQuestionsExtension<ILanguageSetModule, ILanguageQuestions>().Names.SignValueQuestion)
				.SerializeToXml(question => new Xml.SignValueQuestion(question))
				.SerializeToJson(question => new Json.SignValueQuestion(question));

			Repositories.RegisterQuestion<GetCommonQuestion>(language => language.GetQuestionsExtension<ILanguageSetModule, ILanguageQuestions>().Names.GetCommonQuestion)
				.SerializeToXml(question => new Xml.GetCommonQuestion(question))
				.SerializeToJson(question => new Json.GetCommonQuestion(question));
			Repositories.RegisterQuestion<GetDifferencesQuestion>(language => language.GetQuestionsExtension<ILanguageSetModule, ILanguageQuestions>().Names.GetDifferencesQuestion)
				.SerializeToXml(question => new Xml.GetDifferencesQuestion(question))
				.SerializeToJson(question => new Json.GetDifferencesQuestion(question));
			Repositories.RegisterQuestion<WhatQuestion>(language => language.GetQuestionsExtension<ILanguageSetModule, ILanguageQuestions>().Names.WhatQuestion)
				.SerializeToXml(question => new Xml.WhatQuestion(question))
				.SerializeToJson(question => new Json.WhatQuestion(question));
		}

		public override IDictionary<String, Type> GetLanguageExtensions()
		{
			return new Dictionary<String, Type>
			{
				{ nameof(SetModule), typeof(LanguageSetModule) }
			};
		}

		private static async Task CheckSignDuplicationsAsync(
			ISemanticNetwork semanticNetwork,
			ITextContainer result,
			ICollection<HasSignStatement> statements)
		{
			var classifications = await semanticNetwork.Statements.OfType<Classification.Statements.IsStatement>().ToListAsync();

			foreach (var hasSign in statements)
			{
				if (! await hasSign.CheckSignDuplicationAsync(statements, classifications))
				{
					result.Append(
						language => language.GetStatementsExtension<ILanguageSetModule, ILanguageStatements>().Consistency.ErrorMultipleSign,
						new Dictionary<String, IKnowledge> { { AabSemantics.Localization.Strings.ParamStatement, hasSign } });
				}
			}
		}

		private static async Task CheckSignValuesAsync(
			ISemanticNetwork semanticNetwork,
			ITextContainer result,
			ICollection<SignValueStatement> statements)
		{
			await CheckMultiValues(statements, result, semanticNetwork);
			await CheckValuesWithoutSign(statements, result, semanticNetwork);
		}

		private static async Task CheckMultiValues(
			ICollection<SignValueStatement> statements,
			ITextContainer result,
			ISemanticNetwork semanticNetwork)
		{
			var conceptValues = new Dictionary<IConcept, ICollection<IConcept>>();
			foreach (var statement in statements)
			{
				ICollection<IConcept> valuedSigns;
				if (!conceptValues.TryGetValue(statement.Concept, out valuedSigns))
				{
					conceptValues[statement.Concept] = new HashSet<IConcept> { statement.Sign };
				}
				else
				{
					if (valuedSigns.Contains(statement.Sign))
					{
						result.Append(
							language => language.GetStatementsExtension<ILanguageSetModule, ILanguageStatements>().Consistency.ErrorMultipleSignValue,
							new Dictionary<String, IKnowledge>
							{
								{ AabSemantics.Localization.Strings.ParamConcept, statement.Concept },
								{ Strings.ParamSign, statement.Sign },
							});
					}
					else
					{
						valuedSigns.Add(statement.Sign);
					}
				}
			}

			var classifications = await semanticNetwork.Statements.OfType<Classification.Statements.IsStatement>().ToListAsync();

			foreach (var concept in semanticNetwork.Concepts)
			{
				var parents = await classifications.GetParentsOneLevelAsync(concept);
				foreach (var sign in await HasSignStatement.GetSignsAsync(semanticNetwork.Statements, concept, true))
				{
					if (statements.FirstOrDefault(sv => sv.Concept == concept && sv.Sign == sign.Sign) == null &&
						parents.Select(async p => await SignValueStatement.GetSignValueAsync(semanticNetwork.Statements, p, sign.Sign)).Count(r => r != null) > 1)
					{
						result.Append(
							language => language.GetStatementsExtension<ILanguageSetModule, ILanguageStatements>().Consistency.ErrorMultipleSignValueParents,
							new Dictionary<String, IKnowledge>
							{
								{ AabSemantics.Localization.Strings.ParamConcept, concept },
								{ Strings.ParamSign, sign.Sign },
							});
					}
				}
			}
		}

		private static async Task CheckValuesWithoutSign(
			ICollection<SignValueStatement> statements,
			ITextContainer result,
			ISemanticNetwork semanticNetwork)
		{
			foreach (var signValue in statements)
			{
				if (! await signValue.CheckHasSignAsync(semanticNetwork.Statements))
				{
					result.Append(
						language => language.GetStatementsExtension<ILanguageSetModule, ILanguageStatements>().Consistency.ErrorSignWithoutValue,
						new Dictionary<String, IKnowledge> { { AabSemantics.Localization.Strings.ParamStatement, signValue } });
				}
			}
		}
	}
}
