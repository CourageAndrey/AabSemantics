using System;
using System.Collections.Generic;

using AabSemantics.Modules.Set.Localization;
using AabSemantics.Statements;

namespace AabSemantics.Modules.Set.Statements
{
	public static class CustomStatementExtensions
	{
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

		public static GroupStatement ToGroupStatement(this CustomStatement statement)
		{
			return new GroupStatement(
				statement.ID,
				statement.Concepts[Strings.ParamArea],
				statement.Concepts[AabSemantics.Localization.Strings.ParamConcept]);
		}

		public static HasPartStatement ToHasPartStatement(this CustomStatement statement)
		{
			return new HasPartStatement(
				statement.ID,
				statement.Concepts[AabSemantics.Localization.Strings.ParamParent],
				statement.Concepts[AabSemantics.Localization.Strings.ParamChild]);
		}

		public static HasSignStatement ToHasSignStatement(this CustomStatement statement)
		{
			return new HasSignStatement(
				statement.ID,
				statement.Concepts[AabSemantics.Localization.Strings.ParamConcept],
				statement.Concepts[Strings.ParamSign]);
		}

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
