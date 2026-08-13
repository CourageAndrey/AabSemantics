using System;
using System.Collections.Generic;

using AabSemantics.Modules.Processes.Localization;
using AabSemantics.Statements;

namespace AabSemantics.Modules.Processes.Statements
{
	/// <summary>Converts between <see cref="ProcessesStatement"/> and its custom-statement form.</summary>
	public static class CustomStatementExtensions
	{
		/// <summary>Converts a process sequence statement into an equivalent custom statement.</summary>
		/// <param name="statement">Statement to convert.</param>
		/// <returns>A custom statement with the same identifier and role concepts.</returns>
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

		/// <summary>Converts a custom statement back into a process sequence statement.</summary>
		/// <param name="statement">Statement to convert; must carry the process and sign roles.</param>
		/// <returns>The equivalent process sequence statement.</returns>
		/// <exception cref="System.Collections.Generic.KeyNotFoundException">A required role is missing.</exception>
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
