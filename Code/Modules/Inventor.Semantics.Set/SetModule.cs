using System;
using System.Collections.Generic;
using System.Linq;

using Inventor.Semantics.Metadata;
using Inventor.Semantics.Modules.Boolean;
using Inventor.Semantics.Modules.Classification;
using Inventor.Semantics.Modules.Classification.Statements;
using Inventor.Semantics.Set.Attributes;
using Inventor.Semantics.Set.Localization;
using Inventor.Semantics.Set.Questions;
using Inventor.Semantics.Set.Statements;

namespace Inventor.Semantics.Set
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
			Semantics.Localization.Language.Default.Extensions.Add(LanguageSetModule.CreateDefault());
		}

		protected override void RegisterAttributes()
		{
			Repositories.RegisterAttribute(IsSignAttribute.Value, language => language.GetExtension<ILanguageSetModule>().Attributes.IsSign, new Xml.IsSignAttribute());
		}

		protected override void RegisterStatements()
		{
			Repositories.RegisterStatement<HasPartStatement, Xml.HasPartStatement>(
				language => language.GetExtension<ILanguageSetModule>().Statements.Names.Composition,
				statement => new Xml.HasPartStatement(statement),
				StatementDefinition<HasPartStatement>.NoConsistencyCheck);

			Repositories.RegisterStatement<GroupStatement, Xml.GroupStatement>(
				language => language.GetExtension<ILanguageSetModule>().Statements.Names.SubjectArea,
				statement => new Xml.GroupStatement(statement),
				StatementDefinition<GroupStatement>.NoConsistencyCheck);

			Repositories.RegisterStatement<HasSignStatement, Xml.HasSignStatement>(
				language => language.GetExtension<ILanguageSetModule>().Statements.Names.HasSign,
				statement => new Xml.HasSignStatement(statement),
				checkSignDuplications);

			Repositories.RegisterStatement<SignValueStatement, Xml.SignValueStatement>(
				language => language.GetExtension<ILanguageSetModule>().Statements.Names.SignValue,
				statement => new Xml.SignValueStatement(statement),
				checkSignValues);
		}

		protected override void RegisterQuestions()
		{
			Repositories.RegisterQuestion<DescribeSubjectAreaQuestion, Xml.DescribeSubjectAreaQuestion>(
				language => language.GetExtension<ILanguageSetModule>().Questions.Names.DescribeSubjectAreaQuestion,
				question => new Xml.DescribeSubjectAreaQuestion(question));
			Repositories.RegisterQuestion<FindSubjectAreaQuestion, Xml.FindSubjectAreaQuestion>(
				language => language.GetExtension<ILanguageSetModule>().Questions.Names.FindSubjectAreaQuestion,
				question => new Xml.FindSubjectAreaQuestion(question));
			Repositories.RegisterQuestion<IsSubjectAreaQuestion, Xml.IsSubjectAreaQuestion>(
				language => language.GetExtension<ILanguageSetModule>().Questions.Names.IsSubjectAreaQuestion,
				question => new Xml.IsSubjectAreaQuestion(question));

			Repositories.RegisterQuestion<EnumerateContainersQuestion, Xml.EnumerateContainersQuestion>(
				language => language.GetExtension<ILanguageSetModule>().Questions.Names.EnumerateContainersQuestion,
				question => new Xml.EnumerateContainersQuestion(question));
			Repositories.RegisterQuestion<EnumeratePartsQuestion, Xml.EnumeratePartsQuestion>(
				language => language.GetExtension<ILanguageSetModule>().Questions.Names.EnumeratePartsQuestion,
				question => new Xml.EnumeratePartsQuestion(question));
			Repositories.RegisterQuestion<IsPartOfQuestion, Xml.IsPartOfQuestion>(
				language => language.GetExtension<ILanguageSetModule>().Questions.Names.IsPartOfQuestion,
				question => new Xml.IsPartOfQuestion(question));

			Repositories.RegisterQuestion<EnumerateSignsQuestion, Xml.EnumerateSignsQuestion>(
				language => language.GetExtension<ILanguageSetModule>().Questions.Names.EnumerateSignsQuestion,
				question => new Xml.EnumerateSignsQuestion(question));
			Repositories.RegisterQuestion<HasSignQuestion, Xml.HasSignQuestion>(
				language => language.GetExtension<ILanguageSetModule>().Questions.Names.HasSignQuestion,
				question => new Xml.HasSignQuestion(question));
			Repositories.RegisterQuestion<HasSignsQuestion, Xml.HasSignsQuestion>(
				language => language.GetExtension<ILanguageSetModule>().Questions.Names.HasSignsQuestion,
				question => new Xml.HasSignsQuestion(question));
			Repositories.RegisterQuestion<IsSignQuestion, Xml.IsSignQuestion>(
				language => language.GetExtension<ILanguageSetModule>().Questions.Names.IsSignQuestion,
				question => new Xml.IsSignQuestion(question));
			Repositories.RegisterQuestion<IsValueQuestion, Xml.IsValueQuestion>(
				language => language.GetExtension<ILanguageSetModule>().Questions.Names.IsValueQuestion,
				question => new Xml.IsValueQuestion(question));
			Repositories.RegisterQuestion<SignValueQuestion, Xml.SignValueQuestion>(
				language => language.GetExtension<ILanguageSetModule>().Questions.Names.SignValueQuestion,
				question => new Xml.SignValueQuestion(question));

			Repositories.RegisterQuestion<GetCommonQuestion, Xml.GetCommonQuestion>(
				language => language.GetExtension<ILanguageSetModule>().Questions.Names.GetCommonQuestion,
				question => new Xml.GetCommonQuestion(question));
			Repositories.RegisterQuestion<GetDifferencesQuestion, Xml.GetDifferencesQuestion>(
				language => language.GetExtension<ILanguageSetModule>().Questions.Names.GetDifferencesQuestion,
				question => new Xml.GetDifferencesQuestion(question));
			Repositories.RegisterQuestion<WhatQuestion, Xml.WhatQuestion>(
				language => language.GetExtension<ILanguageSetModule>().Questions.Names.WhatQuestion,
				question => new Xml.WhatQuestion(question));
		}

		public override IDictionary<String, Type> GetLanguageExtensions()
		{
			return new Dictionary<String, Type>
			{
				{ nameof(SetModule), typeof(LanguageSetModule) }
			};
		}

		private static void checkSignDuplications(
			ICollection<HasSignStatement> statements,
			ITextContainer result,
			ISemanticNetwork semanticNetwork)
		{
			var clasifications = semanticNetwork.Statements.OfType<IsStatement>().ToList();

			foreach (var hasSign in statements)
			{
				if (!hasSign.CheckSignDuplication(statements, clasifications))
				{
					result.Append(
						language => language.GetExtension<ILanguageSetModule>().Statements.Consistency.ErrorMultipleSign,
						new Dictionary<String, IKnowledge> { { Semantics.Localization.Strings.ParamStatement, hasSign } });
				}
			}
		}

		private static void checkSignValues(
			ICollection<SignValueStatement> statements,
			ITextContainer result,
			ISemanticNetwork semanticNetwork)
		{
			checkMultiValues(statements, result, semanticNetwork);
			checkValuesWithoutSign(statements, result, semanticNetwork);
		}

		private static void checkMultiValues(
			ICollection<SignValueStatement> statements,
			ITextContainer result,
			ISemanticNetwork semanticNetwork)
		{
			var clasifications = semanticNetwork.Statements.OfType<IsStatement>().ToList();

			foreach (var concept in semanticNetwork.Concepts)
			{
				var parents = clasifications.GetParentsOneLevel(concept);
				foreach (var sign in HasSignStatement.GetSigns(semanticNetwork.Statements, concept, true))
				{
					if (statements.FirstOrDefault(sv => sv.Concept == concept && sv.Sign == sign.Sign) == null &&
						parents.Select(p => SignValueStatement.GetSignValue(semanticNetwork.Statements, p, sign.Sign)).Count(r => r != null) > 1)
					{
						result.Append(
							language => language.GetExtension<ILanguageSetModule>().Statements.Consistency.ErrorMultipleSignValue,
							new Dictionary<String, IKnowledge>
							{
								{ Semantics.Localization.Strings.ParamConcept, concept },
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
						new Dictionary<String, IKnowledge> { { Semantics.Localization.Strings.ParamStatement, signValue } });
				}
			}
		}
	}
}
