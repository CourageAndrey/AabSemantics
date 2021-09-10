using System;
using System.Collections.Generic;
using System.Linq;

using Inventor.Core.Attributes;
using Inventor.Core.Localization;
using Inventor.Core.Localization.Modules;
using Inventor.Core.Metadata;
using Inventor.Core.Statements;
using Inventor.Core.Questions;

namespace Inventor.Core.Modules
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
			Language.Default.Extensions.Add(LanguageSetModule.CreateDefault());
		}

		protected override void RegisterAttributes()
		{
			Repositories.RegisterAttribute(IsSignAttribute.Value, language => language.GetExtension<ILanguageSetModule>().Attributes.IsSign, new Xml.IsSignAttribute());
		}

		protected override void RegisterStatements()
		{
			Repositories.RegisterStatement<HasPartStatement>(
				language => language.GetExtension<ILanguageSetModule>().Statements.Names.Composition,
				statement => new Xml.HasPartStatement(statement),
				typeof(Xml.HasPartStatement),
				StatementDefinition<HasPartStatement>.NoConsistencyCheck);

			Repositories.RegisterStatement<GroupStatement>(
				language => language.GetExtension<ILanguageSetModule>().Statements.Names.SubjectArea,
				statement => new Xml.GroupStatement(statement),
				typeof(Xml.GroupStatement),
				StatementDefinition<GroupStatement>.NoConsistencyCheck);

			Repositories.RegisterStatement<HasSignStatement>(
				language => language.GetExtension<ILanguageSetModule>().Statements.Names.HasSign,
				statement => new Xml.HasSignStatement(statement),
				typeof(Xml.HasSignStatement),
				checkSignDuplications);

			Repositories.RegisterStatement<SignValueStatement>(
				language => language.GetExtension<ILanguageSetModule>().Statements.Names.SignValue,
				statement => new Xml.SignValueStatement(statement),
				typeof(Xml.SignValueStatement),
				checkSignValues);
		}

		protected override void RegisterQuestions()
		{
			Repositories.RegisterQuestion<DescribeSubjectAreaQuestion>(language => language.GetExtension<ILanguageSetModule>().Questions.Names.DescribeSubjectAreaQuestion);
			Repositories.RegisterQuestion<FindSubjectAreaQuestion>(language => language.GetExtension<ILanguageSetModule>().Questions.Names.FindSubjectAreaQuestion);
			Repositories.RegisterQuestion<IsSubjectAreaQuestion>(language => language.GetExtension<ILanguageSetModule>().Questions.Names.IsSubjectAreaQuestion);

			Repositories.RegisterQuestion<EnumerateContainersQuestion>(language => language.GetExtension<ILanguageSetModule>().Questions.Names.EnumerateContainersQuestion);
			Repositories.RegisterQuestion<EnumeratePartsQuestion>(language => language.GetExtension<ILanguageSetModule>().Questions.Names.EnumeratePartsQuestion);
			Repositories.RegisterQuestion<IsPartOfQuestion>(language => language.GetExtension<ILanguageSetModule>().Questions.Names.IsPartOfQuestion);

			Repositories.RegisterQuestion<EnumerateSignsQuestion>(language => language.GetExtension<ILanguageSetModule>().Questions.Names.EnumerateSignsQuestion);
			Repositories.RegisterQuestion<HasSignQuestion>(language => language.GetExtension<ILanguageSetModule>().Questions.Names.HasSignQuestion);
			Repositories.RegisterQuestion<HasSignsQuestion>(language => language.GetExtension<ILanguageSetModule>().Questions.Names.HasSignsQuestion);
			Repositories.RegisterQuestion<IsSignQuestion>(language => language.GetExtension<ILanguageSetModule>().Questions.Names.IsSignQuestion);
			Repositories.RegisterQuestion<IsValueQuestion>(language => language.GetExtension<ILanguageSetModule>().Questions.Names.IsValueQuestion);
			Repositories.RegisterQuestion<SignValueQuestion>(language => language.GetExtension<ILanguageSetModule>().Questions.Names.SignValueQuestion);

			Repositories.RegisterQuestion<GetCommonQuestion>(language => language.GetExtension<ILanguageSetModule>().Questions.Names.GetCommonQuestion);
			Repositories.RegisterQuestion<GetDifferencesQuestion>(language => language.GetExtension<ILanguageSetModule>().Questions.Names.GetDifferencesQuestion);
			Repositories.RegisterQuestion<WhatQuestion>(language => language.GetExtension<ILanguageSetModule>().Questions.Names.WhatQuestion);
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
						new Dictionary<String, IKnowledge> { { Strings.ParamStatement, hasSign } });
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
								{ Strings.ParamConcept, concept },
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
						new Dictionary<String, IKnowledge> { { Strings.ParamStatement, signValue } });
				}
			}
		}
	}
}
