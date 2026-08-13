using System;
using System.Collections.Generic;

using AabSemantics.Modules.Set.Localization;
using AabSemantics.Statements;

namespace AabSemantics.Modules.Set.Statements
{
	/// <summary>Converts the set module's statements to and from their custom-statement forms.</summary>
	public static class CustomStatementExtensions
	{
		/// <summary>Converts a <see cref="GroupStatement"/> into an equivalent custom statement.</summary>
		/// <param name="statement">Statement to convert.</param>
		/// <returns>A custom statement with the same identifier and role concepts.</returns>
		public static CustomStatement ToCustomStatement(this GroupStatement statement)
		{
			return new CustomStatement(
				statement.ID,
				typeof(GroupStatement).Name,
				new Dictionary<String, IConcept>
				{
					{ Strings.ParamArea, statement.Area },
					{ AabSemantics.Localization.Strings.ParamConcept, statement.Concept },
				});
		}

		/// <summary>Converts a <see cref="HasPartStatement"/> into an equivalent custom statement.</summary>
		/// <param name="statement">Statement to convert.</param>
		/// <returns>A custom statement with the same identifier and role concepts.</returns>
		public static CustomStatement ToCustomStatement(this HasPartStatement statement)
		{
			return new CustomStatement(
				statement.ID,
				typeof(HasPartStatement).Name,
				new Dictionary<String, IConcept>
				{
					{ AabSemantics.Localization.Strings.ParamParent, statement.Whole },
					{ AabSemantics.Localization.Strings.ParamChild, statement.Part },
				});
		}

		/// <summary>Converts a <see cref="HasSignStatement"/> into an equivalent custom statement.</summary>
		/// <param name="statement">Statement to convert.</param>
		/// <returns>A custom statement with the same identifier and role concepts.</returns>
		public static CustomStatement ToCustomStatement(this HasSignStatement statement)
		{
			return new CustomStatement(
				statement.ID,
				typeof(HasSignStatement).Name,
				new Dictionary<String, IConcept>
				{
					{ AabSemantics.Localization.Strings.ParamConcept, statement.Concept },
					{ Strings.ParamSign, statement.Sign },
				});
		}

		/// <summary>Converts a <see cref="SignValueStatement"/> into an equivalent custom statement.</summary>
		/// <param name="statement">Statement to convert.</param>
		/// <returns>A custom statement with the same identifier and role concepts.</returns>
		public static CustomStatement ToCustomStatement(this SignValueStatement statement)
		{
			return new CustomStatement(
				statement.ID,
				typeof(SignValueStatement).Name,
				new Dictionary<String, IConcept>
				{
					{ AabSemantics.Localization.Strings.ParamConcept, statement.Concept },
					{ Strings.ParamSign, statement.Sign },
					{ Strings.ParamValue, statement.Value },
				});
		}

		/// <summary>Converts a custom statement back into a <see cref="GroupStatement"/>.</summary>
		/// <param name="statement">Statement to convert; must carry the expected roles.</param>
		/// <returns>The equivalent statement.</returns>
		/// <exception cref="System.Collections.Generic.KeyNotFoundException">A required role is missing.</exception>
		public static GroupStatement ToGroupStatement(this CustomStatement statement)
		{
			return new GroupStatement(
				statement.ID,
				statement.Concepts[Strings.ParamArea],
				statement.Concepts[AabSemantics.Localization.Strings.ParamConcept]);
		}

		/// <summary>Converts a custom statement back into a <see cref="HasPartStatement"/>.</summary>
		/// <param name="statement">Statement to convert; must carry the expected roles.</param>
		/// <returns>The equivalent statement.</returns>
		/// <exception cref="System.Collections.Generic.KeyNotFoundException">A required role is missing.</exception>
		public static HasPartStatement ToHasPartStatement(this CustomStatement statement)
		{
			return new HasPartStatement(
				statement.ID,
				statement.Concepts[AabSemantics.Localization.Strings.ParamParent],
				statement.Concepts[AabSemantics.Localization.Strings.ParamChild]);
		}

		/// <summary>Converts a custom statement back into a <see cref="HasSignStatement"/>.</summary>
		/// <param name="statement">Statement to convert; must carry the expected roles.</param>
		/// <returns>The equivalent statement.</returns>
		/// <exception cref="System.Collections.Generic.KeyNotFoundException">A required role is missing.</exception>
		public static HasSignStatement ToHasSignStatement(this CustomStatement statement)
		{
			return new HasSignStatement(
				statement.ID,
				statement.Concepts[AabSemantics.Localization.Strings.ParamConcept],
				statement.Concepts[Strings.ParamSign]);
		}

		/// <summary>Converts a custom statement back into a <see cref="SignValueStatement"/>.</summary>
		/// <param name="statement">Statement to convert; must carry the expected roles.</param>
		/// <returns>The equivalent statement.</returns>
		/// <exception cref="System.Collections.Generic.KeyNotFoundException">A required role is missing.</exception>
		public static SignValueStatement ToSignValueStatement(this CustomStatement statement)
		{
			return new SignValueStatement(
				statement.ID,
				statement.Concepts[AabSemantics.Localization.Strings.ParamConcept],
				statement.Concepts[Strings.ParamSign],
				statement.Concepts[Strings.ParamValue]);
		}
	}
}
