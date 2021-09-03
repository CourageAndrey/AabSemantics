using System;
using System.Collections.Generic;
using System.Linq;

using Inventor.Core.Attributes;
using Inventor.Core.Localization;
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
		{
			Repositories.RegisterAttribute(IsSignAttribute.Value, language => language.Attributes.IsSign, new Xml.IsSignAttribute());

			Repositories.RegisterStatement<HasPartStatement>(
				language => language.StatementNames.Composition,
				statement => new Xml.HasPartStatement(statement),
				typeof(Xml.HasPartStatement),
				StatementDefinition<HasPartStatement>.NoConsistencyCheck);
			Repositories.RegisterStatement<GroupStatement>(
				language => language.StatementNames.SubjectArea,
				statement => new Xml.GroupStatement(statement),
				typeof(Xml.GroupStatement),
				StatementDefinition<GroupStatement>.NoConsistencyCheck);
			Repositories.RegisterStatement<HasSignStatement>(
				language => language.StatementNames.HasSign,
				statement => new Xml.HasSignStatement(statement),
				typeof(Xml.HasSignStatement),
				(statements, result, sn) =>
				{
					var clasifications = sn.Statements.OfType<IsStatement>().ToList();

					foreach (var hasSign in statements)
					{
						if (!hasSign.CheckSignDuplication(statements, clasifications))
						{
							result.Append(
								language => language.Consistency.ErrorMultipleSign,
								new Dictionary<String, IKnowledge> { { Strings.ParamStatement, hasSign } });
						}
					}
				});
			Repositories.RegisterStatement<SignValueStatement>(
				language => language.StatementNames.SignValue,
				statement => new Xml.SignValueStatement(statement),
				typeof(Xml.SignValueStatement),
				(statements, result, sn) =>
				{
					var clasifications = sn.Statements.OfType<IsStatement>().ToList();

					// 3. check multi values
					foreach (var concept in semanticNetwork.Concepts)
					{
						var parents = clasifications.GetParentsOneLevel(concept);
						foreach (var sign in HasSignStatement.GetSigns(semanticNetwork.Statements, concept, true))
						{
							if (statements.FirstOrDefault(sv => sv.Concept == concept && sv.Sign == sign.Sign) == null &&
								parents.Select(p => SignValueStatement.GetSignValue(semanticNetwork.Statements, p, sign.Sign)).Count(r => r != null) > 1)
							{
								result.Append(
									language => language.Consistency.ErrorMultipleSignValue,
									new Dictionary<String, IKnowledge>
									{
										{ Strings.ParamConcept, concept },
										{ Strings.ParamSign, sign.Sign },
									});
							}
						}
					}

					// 4. check values without sign
					foreach (var signValue in statements)
					{
						if (!signValue.CheckHasSign(semanticNetwork.Statements))
						{
							result.Append(
								language => language.Consistency.ErrorSignWithoutValue,
								new Dictionary<String, IKnowledge> { { Strings.ParamStatement, signValue } });
						}
					}
				});

			Repositories.RegisterQuestion<DescribeSubjectAreaQuestion>();
			Repositories.RegisterQuestion<FindSubjectAreaQuestion>();
			Repositories.RegisterQuestion<IsSubjectAreaQuestion>();

			Repositories.RegisterQuestion<EnumerateContainersQuestion>();
			Repositories.RegisterQuestion<EnumeratePartsQuestion>();
			Repositories.RegisterQuestion<IsPartOfQuestion>();

			Repositories.RegisterQuestion<EnumerateSignsQuestion>();
			Repositories.RegisterQuestion<HasSignQuestion>();
			Repositories.RegisterQuestion<HasSignsQuestion>();
			Repositories.RegisterQuestion<IsSignQuestion>();
			Repositories.RegisterQuestion<IsValueQuestion>();
			Repositories.RegisterQuestion<SignValueQuestion>();

			Repositories.RegisterQuestion<GetCommonQuestion>();
			Repositories.RegisterQuestion<GetDifferencesQuestion>();
			Repositories.RegisterQuestion<WhatQuestion>();
		}
	}
}
