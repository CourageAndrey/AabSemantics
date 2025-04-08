using System;
using System.Collections.Generic;

using AabSemantics.Localization;
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
				new Dictionary<String, IConcept>
				{
					{ Strings.ParamParent, statement.Ancestor },
					{ Strings.ParamChild, statement.Descendant },
				});
		}

		public static IsStatement ToIsStatement(this CustomStatement statement)
		{
			return new IsStatement(
				statement.ID,
				statement.Concepts[Strings.ParamParent],
				statement.Concepts[Strings.ParamChild]);
		}
	}
}
