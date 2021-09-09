using System;
using System.Collections.Generic;
using System.Linq;

using Inventor.Core.Attributes;
using Inventor.Core.Localization;
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

		protected override void RegisterAttributes()
		{
			Repositories.RegisterAttribute(IsSignAttribute.Value, language => language.Attributes.IsSign, new Xml.IsSignAttribute());
		}

		protected override void RegisterStatements()
		{
			Repositories.RegisterStatement<HasPartStatement>(
				language => language.Statements.Names.Composition,
				statement => new Xml.HasPartStatement(statement),
				typeof(Xml.HasPartStatement),
				StatementDefinition<HasPartStatement>.NoConsistencyCheck);

			Repositories.RegisterStatement<GroupStatement>(
				language => language.Statements.Names.SubjectArea,
				statement => new Xml.GroupStatement(statement),
				typeof(Xml.GroupStatement),
				StatementDefinition<GroupStatement>.NoConsistencyCheck);

			Repositories.RegisterStatement<HasSignStatement>(
				language => language.Statements.Names.HasSign,
				statement => new Xml.HasSignStatement(statement),
				typeof(Xml.HasSignStatement),
				checkSignDuplications);

			Repositories.RegisterStatement<SignValueStatement>(
				language => language.Statements.Names.SignValue,
				statement => new Xml.SignValueStatement(statement),
				typeof(Xml.SignValueStatement),
				checkSignValues);
		}

		protected override void RegisterQuestions()
		{
			Repositories.RegisterQuestion<DescribeSubjectAreaQuestion>(language => language.Questions.Names.DescribeSubjectAreaQuestion);
			Repositories.RegisterQuestion<FindSubjectAreaQuestion>(language => language.Questions.Names.FindSubjectAreaQuestion);
			Repositories.RegisterQuestion<IsSubjectAreaQuestion>(language => language.Questions.Names.IsSubjectAreaQuestion);

			Repositories.RegisterQuestion<EnumerateContainersQuestion>(language => language.Questions.Names.EnumerateContainersQuestion);
			Repositories.RegisterQuestion<EnumeratePartsQuestion>(language => language.Questions.Names.EnumeratePartsQuestion);
			Repositories.RegisterQuestion<IsPartOfQuestion>(language => language.Questions.Names.IsPartOfQuestion);

			Repositories.RegisterQuestion<EnumerateSignsQuestion>(language => language.Questions.Names.EnumerateSignsQuestion);
			Repositories.RegisterQuestion<HasSignQuestion>(language => language.Questions.Names.HasSignQuestion);
			Repositories.RegisterQuestion<HasSignsQuestion>(language => language.Questions.Names.HasSignsQuestion);
			Repositories.RegisterQuestion<IsSignQuestion>(language => language.Questions.Names.IsSignQuestion);
			Repositories.RegisterQuestion<IsValueQuestion>(language => language.Questions.Names.IsValueQuestion);
			Repositories.RegisterQuestion<SignValueQuestion>(language => language.Questions.Names.SignValueQuestion);

			Repositories.RegisterQuestion<GetCommonQuestion>(language => language.Questions.Names.GetCommonQuestion);
			Repositories.RegisterQuestion<GetDifferencesQuestion>(language => language.Questions.Names.GetDifferencesQuestion);
			Repositories.RegisterQuestion<WhatQuestion>(language => language.Questions.Names.WhatQuestion);
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
						language => language.Statements.Consistency.ErrorMultipleSign,
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
							language => language.Statements.Consistency.ErrorMultipleSignValue,
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
						language => language.Statements.Consistency.ErrorSignWithoutValue,
						new Dictionary<String, IKnowledge> { { Strings.ParamStatement, signValue } });
				}
			}
		}
	}
}
