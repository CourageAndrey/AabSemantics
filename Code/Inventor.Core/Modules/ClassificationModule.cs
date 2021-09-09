using System;
using System.Collections.Generic;

using Inventor.Core.Localization;
using Inventor.Core.Metadata;
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
				language => language.Statements.Names.Clasification,
				statement => new Xml.IsStatement(statement),
				typeof(Xml.IsStatement),
				checkCyclicParents);
		}

		protected override void RegisterQuestions()
		{
			Repositories.RegisterQuestion<EnumerateAncestorsQuestion>(language => language.Questions.Names.EnumerateAncestorsQuestion);
			Repositories.RegisterQuestion<EnumerateDescendantsQuestion>(language => language.Questions.Names.EnumerateDescendantsQuestion);
			Repositories.RegisterQuestion<IsQuestion>(language => language.Questions.Names.IsQuestion);
		}

		private static void checkCyclicParents(
			ICollection<IsStatement> statements,
			ITextContainer result,
			ISemanticNetwork semanticNetwork)
		{
			foreach (var clasification in statements)
			{
				if (!clasification.CheckCyclic(statements))
				{
					result.Append(
						language => language.Statements.Consistency.ErrorCyclic,
						new Dictionary<String, IKnowledge> { { Strings.ParamStatement, clasification } });
				}
			}
		}
	}
}
