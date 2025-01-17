using System;
using System.Collections.Generic;

using AabSemantics.Localization;
using AabSemantics.Modules.Classification.Localization;
using AabSemantics.Statements;

namespace AabSemantics.Modules.Classification.Statements
{
	public static class CustomStatementExtensions
	{
		public static CustomStatement ToCustomStatement(this IsStatement statement)
		{
			return new CustomStatement(
				statement.ID,
				typeof(IsStatement).Name,
				language => language.GetExtension<ILanguageClassificationModule>().Statements.TrueFormatStrings.Clasification,
				language => language.GetExtension<ILanguageClassificationModule>().Statements.FalseFormatStrings.Clasification,
				language => language.GetExtension<ILanguageClassificationModule>().Statements.QuestionFormatStrings.Clasification,
				statement.Name as LocalizedString,
				statement.Hint as LocalizedString,
				new Dictionary<String, IConcept>
				{
					{ nameof(IsStatement.Ancestor), statement.Ancestor },
					{ nameof(IsStatement.Descendant), statement.Descendant },
				});
		}

		public static IsStatement ToIsStatement(this CustomStatement statement)
		{
			return new IsStatement(
				statement.ID,
				statement.Concepts[nameof(IsStatement.Ancestor)],
				statement.Concepts[nameof(IsStatement.Descendant)]);
		}
	}
}
