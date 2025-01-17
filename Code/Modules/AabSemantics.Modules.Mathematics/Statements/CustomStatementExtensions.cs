using System;
using System.Collections.Generic;

using AabSemantics.Statements;

namespace AabSemantics.Modules.Mathematics.Statements
{
	public static class CustomStatementExtensions
	{
		public static CustomStatement ToCustomStatement(this ComparisonStatement statement)
		{
			return new CustomStatement(
				statement.ID,
				typeof(ComparisonStatement).Name,
				new Dictionary<String, IConcept>
				{
					{ nameof(ComparisonStatement.LeftValue), statement.LeftValue },
					{ nameof(ComparisonStatement.RightValue), statement.RightValue },
					{ nameof(ComparisonStatement.ComparisonSign), statement.ComparisonSign },
				});
		}

		public static ComparisonStatement ToComparisonStatement(this CustomStatement statement)
		{
			return new ComparisonStatement(
				statement.ID,
				statement.Concepts[nameof(ComparisonStatement.LeftValue)],
				statement.Concepts[nameof(ComparisonStatement.RightValue)],
				statement.Concepts[nameof(ComparisonStatement.ComparisonSign)]);
		}
	}
}
