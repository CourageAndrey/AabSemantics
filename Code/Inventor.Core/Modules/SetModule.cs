using System;

using Inventor.Core.Attributes;
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
				typeof(Xml.HasPartStatement));
			Repositories.RegisterStatement<GroupStatement>(
				language => language.StatementNames.SubjectArea,
				statement => new Xml.GroupStatement(statement),
				typeof(Xml.GroupStatement));
			Repositories.RegisterStatement<HasSignStatement>(
				language => language.StatementNames.HasSign,
				statement => new Xml.HasSignStatement(statement),
				typeof(Xml.HasSignStatement));
			Repositories.RegisterStatement<SignValueStatement>(
				language => language.StatementNames.SignValue,
				statement => new Xml.SignValueStatement(statement),
				typeof(Xml.SignValueStatement));

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
