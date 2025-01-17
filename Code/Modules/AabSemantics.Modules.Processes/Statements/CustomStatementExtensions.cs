using System;
using System.Collections.Generic;

using AabSemantics.Localization;
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
				language => language.GetExtension<ILanguageProcessesModule>().Statements.TrueFormatStrings.Processes,
				language => language.GetExtension<ILanguageProcessesModule>().Statements.FalseFormatStrings.Processes,
				language => language.GetExtension<ILanguageProcessesModule>().Statements.QuestionFormatStrings.Processes,
				statement.Name as LocalizedString,
				statement.Hint as LocalizedString,
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
