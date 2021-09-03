using System;
using System.Collections.Generic;

using Inventor.Core.Attributes;
using Inventor.Core.Localization;
using Inventor.Core.Statements;
using Inventor.Core.Questions;

namespace Inventor.Core.Modules
{
	public class ProcessesModule : ExtensionModule
	{
		public const String ModuleName = "System.Processes";

		public ProcessesModule()
			: base(ModuleName)
		{ }

		protected override void Attach(ISemanticNetwork semanticNetwork)
		{
			foreach (var sign in SequenceSigns.All)
			{
				semanticNetwork.Concepts.Add(sign);
			}
		}

		protected override void RegisterAttributes()
		{
			Repositories.RegisterAttribute(IsProcessAttribute.Value, language => language.Attributes.IsProcess, new Xml.IsProcessAttribute());
			Repositories.RegisterAttribute(IsSequenceSignAttribute.Value, language => language.Attributes.IsSequenceSign, new Xml.IsSequenceSignAttribute());
		}

		protected override void RegisterStatements()
		{
			Repositories.RegisterStatement<ProcessesStatement>(
				language => language.StatementNames.Processes,
				statement => new Xml.ProcessesStatement(statement),
				typeof(Xml.ProcessesStatement),
				(statements, result, semanticNetwork) =>
				{
					foreach (var contradiction in statements.CheckForContradictions())
					{
						result
							.Append(
								language => language.Consistency.ErrorProcessesContradiction,
								new Dictionary<String, IKnowledge>
								{
									{ Strings.ParamProcessA, contradiction.Value1 },
									{ Strings.ParamProcessB, contradiction.Value2 },
								})
							.Append(contradiction.Signs.EnumerateOneLine());
					}
				});
		}

		protected override void RegisterQuestions()
		{
			Repositories.RegisterQuestion<ProcessesQuestion>();
		}
	}
}
