using System;
using System.Collections.Generic;

using AabSemantics.Modules.Processes.Localization;
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
					{ Strings.ParamProcessA, statement.ProcessA },
					{ Strings.ParamProcessB, statement.ProcessB },
					{ Strings.ParamSequenceSign, statement.SequenceSign },
				});
		}

		public static ProcessesStatement ToProcessesStatement(this CustomStatement statement)
		{
			return new ProcessesStatement(
				statement.ID,
				statement.Concepts[Strings.ParamProcessA],
				statement.Concepts[Strings.ParamProcessB],
				statement.Concepts[Strings.ParamSequenceSign]);
		}
	}
}
