using System;
using System.Collections.Generic;

using AabSemantics.Modules.Mathematics.Localization;
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
					{ Strings.ParamLeftValue, statement.LeftValue },
					{ Strings.ParamRightValue, statement.RightValue },
					{ Strings.ParamComparisonSign, statement.ComparisonSign },
				});
		}

		public static ComparisonStatement ToComparisonStatement(this CustomStatement statement)
		{
			return new ComparisonStatement(
				statement.ID,
				statement.Concepts[Strings.ParamLeftValue],
				statement.Concepts[Strings.ParamRightValue],
				statement.Concepts[Strings.ParamComparisonSign]);
		}
	}
}
