using System;
using System.Collections.Generic;
using System.Linq;

using AabSemantics.Metadata;
using AabSemantics.Modules.Boolean;
using AabSemantics.Modules.Classification;
using AabSemantics.Modules.Classification.Statements;
using AabSemantics.Modules.Set.Attributes;
using AabSemantics.Modules.Set.Localization;
using AabSemantics.Modules.Set.Questions;
using AabSemantics.Modules.Set.Statements;

namespace AabSemantics.Modules.Set
{
	public class SetModule : ExtensionModule
	{
		public const String ModuleName = "System.Sets";

		public SetModule()
			: base(ModuleName, new[] { BooleanModule.ModuleName, ClassificationModule.ModuleName })
		{ }

		protected override void Attach(ISemanticNetwork semanticNetwork)
		{ }

		protected override void RegisterLanguage()
		{
			AabSemantics.Localization.Language.Default.Extensions.Add(LanguageSetModule.CreateDefault());
		}

		protected override void RegisterAttributes()
		{
			Repositories.RegisterAttribute(IsSignAttribute.Value, language => language.GetExtension<ILanguageSetModule>().Attributes.IsSign)
				.SerializeToXml(new Xml.IsSignAttribute())
				.SerializeToJson(new Xml.IsSignAttribute());
		}

		protected override void RegisterStatements()
		{
			Repositories.RegisterStatement<HasPartStatement>(language => language.GetExtension<ILanguageSetModule>().Statements.Names.Composition, StatementDefinition<HasPartStatement>.NoConsistencyCheck)
				.SerializeToXml(statement => new Xml.HasPartStatement(statement))
				.SerializeToJson(statement => new Json.HasPartStatement(statement));

			Repositories.RegisterStatement<GroupStatement>(language => language.GetExtension<ILanguageSetModule>().Statements.Names.SubjectArea, StatementDefinition<GroupStatement>.NoConsistencyCheck)
				.SerializeToXml(statement => new Xml.GroupStatement(statement))
				.SerializeToJson(statement => new Json.GroupStatement(statement));

			Repositories.RegisterStatement<HasSignStatement>(language => language.GetExtension<ILanguageSetModule>().Statements.Names.HasSign, checkSignDuplications)
				.SerializeToXml(statement => new Xml.HasSignStatement(statement))
				.SerializeToJson(statement => new Json.HasSignStatement(statement));

			Repositories.RegisterStatement<SignValueStatement>(language => language.GetExtension<ILanguageSetModule>().Statements.Names.SignValue, checkSignValues)
				.SerializeToXml(statement => new Xml.SignValueStatement(statement))
				.SerializeToJson(statement => new Json.SignValueStatement(statement));
		}

		protected override void RegisterQuestions()
		{
			Repositories.RegisterQuestion<DescribeSubjectAreaQuestion>(language => language.GetExtension<ILanguageSetModule>().Questions.Names.DescribeSubjectAreaQuestion)
				.SerializeToXml(question => new Xml.DescribeSubjectAreaQuestion(question))
				.SerializeToJson(question => new Json.DescribeSubjectAreaQuestion(question));
			Repositories.RegisterQuestion<FindSubjectAreaQuestion>(language => language.GetExtension<ILanguageSetModule>().Questions.Names.FindSubjectAreaQuestion)
				.SerializeToXml(question => new Xml.FindSubjectAreaQuestion(question))
				.SerializeToJson(question => new Json.FindSubjectAreaQuestion(question));
			Repositories.RegisterQuestion<IsSubjectAreaQuestion>(language => language.GetExtension<ILanguageSetModule>().Questions.Names.IsSubjectAreaQuestion)
				.SerializeToXml(question => new Xml.IsSubjectAreaQuestion(question))
				.SerializeToJson(question => new Json.IsSubjectAreaQuestion(question));

			Repositories.RegisterQuestion<EnumerateContainersQuestion>(language => language.GetExtension<ILanguageSetModule>().Questions.Names.EnumerateContainersQuestion)
				.SerializeToXml(question => new Xml.EnumerateContainersQuestion(question))
				.SerializeToJson(question => new Json.EnumerateContainersQuestion(question));
			Repositories.RegisterQuestion<EnumeratePartsQuestion>(language => language.GetExtension<ILanguageSetModule>().Questions.Names.EnumeratePartsQuestion)
				.SerializeToXml(question => new Xml.EnumeratePartsQuestion(question))
				.SerializeToJson(question => new Json.EnumeratePartsQuestion(question));
			Repositories.RegisterQuestion<IsPartOfQuestion>(language => language.GetExtension<ILanguageSetModule>().Questions.Names.IsPartOfQuestion)
				.SerializeToXml(question => new Xml.IsPartOfQuestion(question))
				.SerializeToJson(question => new Json.IsPartOfQuestion(question));

			Repositories.RegisterQuestion<EnumerateSignsQuestion>(language => language.GetExtension<ILanguageSetModule>().Questions.Names.EnumerateSignsQuestion)
				.SerializeToXml(question => new Xml.EnumerateSignsQuestion(question))
				.SerializeToJson(question => new Json.EnumerateSignsQuestion(question));
			Repositories.RegisterQuestion<HasSignQuestion>(language => language.GetExtension<ILanguageSetModule>().Questions.Names.HasSignQuestion)
				.SerializeToXml(question => new Xml.HasSignQuestion(question))
				.SerializeToJson(question => new Json.HasSignQuestion(question));
			Repositories.RegisterQuestion<HasSignsQuestion>(language => language.GetExtension<ILanguageSetModule>().Questions.Names.HasSignsQuestion)
				.SerializeToXml(question => new Xml.HasSignsQuestion(question))
				.SerializeToJson(question => new Json.HasSignsQuestion(question));
			Repositories.RegisterQuestion<IsSignQuestion>(language => language.GetExtension<ILanguageSetModule>().Questions.Names.IsSignQuestion)
				.SerializeToXml(question => new Xml.IsSignQuestion(question))
				.SerializeToJson(question => new Json.IsSignQuestion(question));
			Repositories.RegisterQuestion<IsValueQuestion>(language => language.GetExtension<ILanguageSetModule>().Questions.Names.IsValueQuestion)
				.SerializeToXml(question => new Xml.IsValueQuestion(question))
				.SerializeToJson(question => new Json.IsValueQuestion(question));
			Repositories.RegisterQuestion<SignValueQuestion>(language => language.GetExtension<ILanguageSetModule>().Questions.Names.SignValueQuestion)
				.SerializeToXml(question => new Xml.SignValueQuestion(question))
				.SerializeToJson(question => new Json.SignValueQuestion(question));

			Repositories.RegisterQuestion<GetCommonQuestion>(language => language.GetExtension<ILanguageSetModule>().Questions.Names.GetCommonQuestion)
				.SerializeToXml(question => new Xml.GetCommonQuestion(question))
				.SerializeToJson(question => new Json.GetCommonQuestion(question));
			Repositories.RegisterQuestion<GetDifferencesQuestion>(language => language.GetExtension<ILanguageSetModule>().Questions.Names.GetDifferencesQuestion)
				.SerializeToXml(question => new Xml.GetDifferencesQuestion(question))
				.SerializeToJson(question => new Json.GetDifferencesQuestion(question));
			Repositories.RegisterQuestion<WhatQuestion>(language => language.GetExtension<ILanguageSetModule>().Questions.Names.WhatQuestion)
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

		private static void checkSignDuplications(
			ISemanticNetwork semanticNetwork,
			ITextContainer result,
			ICollection<HasSignStatement> statements)
		{
			var classifications = semanticNetwork.Statements.OfType<IsStatement>().ToList();

			foreach (var hasSign in statements)
			{
				if (!hasSign.CheckSignDuplication(statements, classifications))
				{
					result.Append(
						language => language.GetExtension<ILanguageSetModule>().Statements.Consistency.ErrorMultipleSign,
						new Dictionary<String, IKnowledge> { { AabSemantics.Localization.Strings.ParamStatement, hasSign } });
				}
			}
		}

		private static void checkSignValues(
			ISemanticNetwork semanticNetwork,
			ITextContainer result,
			ICollection<SignValueStatement> statements)
		{
			checkMultiValues(statements, result, semanticNetwork);
			checkValuesWithoutSign(statements, result, semanticNetwork);
		}

		private static void checkMultiValues(
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
							language => language.GetExtension<ILanguageSetModule>().Statements.Consistency.ErrorMultipleSignValue,
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

			var classifications = semanticNetwork.Statements.OfType<IsStatement>().ToList();

			foreach (var concept in semanticNetwork.Concepts)
			{
				var parents = classifications.GetParentsOneLevel(concept);
				foreach (var sign in HasSignStatement.GetSigns(semanticNetwork.Statements, concept, true))
				{
					if (statements.FirstOrDefault(sv => sv.Concept == concept && sv.Sign == sign.Sign) == null &&
						parents.Select(p => SignValueStatement.GetSignValue(semanticNetwork.Statements, p, sign.Sign)).Count(r => r != null) > 1)
					{
						result.Append(
							language => language.GetExtension<ILanguageSetModule>().Statements.Consistency.ErrorMultipleSignValue,
							new Dictionary<String, IKnowledge>
							{
								{ AabSemantics.Localization.Strings.ParamConcept, concept },
								{ Strings.ParamSign, sign.Sign },
							});
					}
				}
			}
		}

		private static void checkValuesWithoutSign(
			ICollection<SignValueStatement> statements,
			ITextContainer result,
			ISemanticNetwork semanticNetwork)
		{
			foreach (var signValue in statements)
			{
				if (!signValue.CheckHasSign(semanticNetwork.Statements))
				{
					result.Append(
						language => language.GetExtension<ILanguageSetModule>().Statements.Consistency.ErrorSignWithoutValue,
						new Dictionary<String, IKnowledge> { { AabSemantics.Localization.Strings.ParamStatement, signValue } });
				}
			}
		}
	}
}
