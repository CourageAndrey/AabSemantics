using System;
using System.Collections.Generic;

using Inventor.Core.Localization;
using Inventor.Core.Statements;
using Inventor.Core.Questions;

namespace Inventor.Core.Modules
{
	public class ClassificationModule : ExtensionModule
	{
		public const String ModuleName = "System.Classification";

		public ClassificationModule()
			: base(ModuleName, new[] { BooleanModule.ModuleName })
		{ }

		protected override void RegisterStatements()
		{
			Repositories.RegisterStatement<IsStatement>(
				language => language.StatementNames.Clasification,
				statement => new Xml.IsStatement(statement),
				typeof(Xml.IsStatement),
				(statements, result, semanticNetwork) =>
				{
					foreach (var clasification in statements)
					{
						if (!clasification.CheckCyclic(statements))
						{
							result.Append(
								language => language.Consistency.ErrorCyclic,
								new Dictionary<String, IKnowledge> { { Strings.ParamStatement, clasification } });
						}
					}
				});
		}

		protected override void RegisterQuestions()
		{
			Repositories.RegisterQuestion<EnumerateAncestorsQuestion>();
			Repositories.RegisterQuestion<EnumerateDescendantsQuestion>();
			Repositories.RegisterQuestion<IsQuestion>();
		}
	}
}
