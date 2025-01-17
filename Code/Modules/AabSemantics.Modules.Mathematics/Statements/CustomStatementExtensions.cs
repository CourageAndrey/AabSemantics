using System;
using System.Collections.Generic;

using AabSemantics.Localization;
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
				language => language.GetExtension<ILanguageMathematicsModule>().Statements.TrueFormatStrings.Comparison,
				language => language.GetExtension<ILanguageMathematicsModule>().Statements.FalseFormatStrings.Comparison,
				language => language.GetExtension<ILanguageMathematicsModule>().Statements.QuestionFormatStrings.Comparison,
				statement.Name as LocalizedString,
				statement.Hint as LocalizedString,
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
