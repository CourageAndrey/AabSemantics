using System;
using System.Collections.Generic;

using AabSemantics.Modules.Mathematics.Localization;
using AabSemantics.Statements;

namespace AabSemantics.Modules.Mathematics.Statements
{
	/// <summary>Converts between <see cref="ComparisonStatement"/> and its custom-statement form.</summary>
	public static class CustomStatementExtensions
	{
		/// <summary>Converts a comparison statement into an equivalent custom statement.</summary>
		/// <param name="statement">Statement to convert.</param>
		/// <returns>A custom statement with the same identifier and role concepts.</returns>
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

		/// <summary>Converts a custom statement back into a comparison statement.</summary>
		/// <param name="statement">Statement to convert; must carry the value and sign roles.</param>
		/// <returns>The equivalent comparison statement.</returns>
		/// <exception cref="System.Collections.Generic.KeyNotFoundException">A required role is missing.</exception>
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
