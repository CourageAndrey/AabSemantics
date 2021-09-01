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
			semanticNetwork.RegisterAttribute(IsSignAttribute.Value, language => language.Attributes.IsSign, new Xml.IsSignAttribute());

			semanticNetwork.RegisterStatement<HasPartStatement>(language => language.StatementNames.Composition);
			semanticNetwork.RegisterStatement<GroupStatement>(language => language.StatementNames.SubjectArea);
			semanticNetwork.RegisterStatement<HasSignStatement>(language => language.StatementNames.HasSign);
			semanticNetwork.RegisterStatement<SignValueStatement>(language => language.StatementNames.SignValue);

			semanticNetwork.RegisterQuestion<DescribeSubjectAreaQuestion>();
			semanticNetwork.RegisterQuestion<FindSubjectAreaQuestion>();
			semanticNetwork.RegisterQuestion<IsSubjectAreaQuestion>();

			semanticNetwork.RegisterQuestion<EnumerateContainersQuestion>();
			semanticNetwork.RegisterQuestion<EnumeratePartsQuestion>();
			semanticNetwork.RegisterQuestion<IsPartOfQuestion>();

			semanticNetwork.RegisterQuestion<EnumerateSignsQuestion>();
			semanticNetwork.RegisterQuestion<HasSignQuestion>();
			semanticNetwork.RegisterQuestion<HasSignsQuestion>();
			semanticNetwork.RegisterQuestion<IsSignQuestion>();
			semanticNetwork.RegisterQuestion<IsValueQuestion>();
			semanticNetwork.RegisterQuestion<SignValueQuestion>();

			semanticNetwork.RegisterQuestion<GetCommonQuestion>();
			semanticNetwork.RegisterQuestion<GetDifferencesQuestion>();
			semanticNetwork.RegisterQuestion<WhatQuestion>();
		}
	}
}
