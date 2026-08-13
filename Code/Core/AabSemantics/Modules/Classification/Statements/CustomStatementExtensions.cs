using System;
using System.Collections.Generic;

using AabSemantics.Localization;
using AabSemantics.Statements;

namespace AabSemantics.Modules.Classification.Statements
{
	/// <summary>
	/// Converts between the compiled <see cref="IsStatement"/> and its custom-statement form,
	/// which the module registers under the same kind identifier.
	/// </summary>
	public static class CustomStatementExtensions
	{
		/// <summary>Converts an "is a" statement into an equivalent custom statement.</summary>
		/// <param name="statement">Statement to convert.</param>
		/// <returns>A custom statement with the same identifier and role concepts.</returns>
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

		/// <summary>Converts a custom statement back into an "is a" statement.</summary>
		/// <param name="statement">Statement to convert; must carry the parent and child roles.</param>
		/// <returns>The equivalent "is a" statement.</returns>
		/// <exception cref="KeyNotFoundException">A required role is missing.</exception>
		public static IsStatement ToIsStatement(this CustomStatement statement)
		{
			return new IsStatement(
				statement.ID,
				statement.Concepts[Strings.ParamParent],
				statement.Concepts[Strings.ParamChild]);
		}
	}
}
