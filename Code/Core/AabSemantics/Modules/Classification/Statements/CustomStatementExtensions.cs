using System;
using System.Collections.Generic;

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
