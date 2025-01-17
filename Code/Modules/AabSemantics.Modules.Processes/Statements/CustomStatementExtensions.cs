using System;
using System.Collections.Generic;

using AabSemantics.Statements;

namespace AabSemantics.Modules.Processes.Statements
{
	public static class CustomStatementExtensions
	{
		public static CustomStatement ToCustomStatement(this ProcessesStatement statement)
		{
			return new CustomStatement(
				statement.ID,
				typeof(ProcessesStatement).Name,
				new Dictionary<String, IConcept>
				{
					{ nameof(ProcessesStatement.ProcessA), statement.ProcessA },
					{ nameof(ProcessesStatement.ProcessB), statement.ProcessB },
					{ nameof(ProcessesStatement.SequenceSign), statement.SequenceSign },
				});
		}

		public static ProcessesStatement ToProcessesStatement(this CustomStatement statement)
		{
			return new ProcessesStatement(
				statement.ID,
				statement.Concepts[nameof(ProcessesStatement.ProcessA)],
				statement.Concepts[nameof(ProcessesStatement.ProcessB)],
				statement.Concepts[nameof(ProcessesStatement.SequenceSign)]);
		}
	}
}
