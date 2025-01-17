using System;
using System.Collections.Generic;

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
					{ nameof(GroupStatement.Area), statement.Area },
					{ nameof(GroupStatement.Concept), statement.Concept },
				});
		}

		public static CustomStatement ToCustomStatement(this HasPartStatement statement)
		{
			return new CustomStatement(
				statement.ID,
				typeof(HasPartStatement).Name,
				new Dictionary<String, IConcept>
				{
					{ nameof(HasPartStatement.Whole), statement.Whole },
					{ nameof(HasPartStatement.Part), statement.Part },
				});
		}

		public static CustomStatement ToCustomStatement(this HasSignStatement statement)
		{
			return new CustomStatement(
				statement.ID,
				typeof(HasSignStatement).Name,
				new Dictionary<String, IConcept>
				{
					{ nameof(HasSignStatement.Concept), statement.Concept },
					{ nameof(HasSignStatement.Sign), statement.Sign },
				});
		}

		public static CustomStatement ToCustomStatement(this SignValueStatement statement)
		{
			return new CustomStatement(
				statement.ID,
				typeof(SignValueStatement).Name,
				new Dictionary<String, IConcept>
				{
					{ nameof(SignValueStatement.Concept), statement.Concept },
					{ nameof(SignValueStatement.Sign), statement.Sign },
					{ nameof(SignValueStatement.Value), statement.Value },
				});
		}

		public static GroupStatement ToGroupStatement(this CustomStatement statement)
		{
			return new GroupStatement(
				statement.ID,
				statement.Concepts[nameof(GroupStatement.Area)],
				statement.Concepts[nameof(GroupStatement.Concept)]);
		}

		public static HasPartStatement ToHasPartStatement(this CustomStatement statement)
		{
			return new HasPartStatement(
				statement.ID,
				statement.Concepts[nameof(HasPartStatement.Whole)],
				statement.Concepts[nameof(HasPartStatement.Part)]);
		}

		public static HasSignStatement ToHasSignStatement(this CustomStatement statement)
		{
			return new HasSignStatement(
				statement.ID,
				statement.Concepts[nameof(HasSignStatement.Concept)],
				statement.Concepts[nameof(HasSignStatement.Sign)]);
		}

		public static SignValueStatement ToSignValueStatement(this CustomStatement statement)
		{
			return new SignValueStatement(
				statement.ID,
				statement.Concepts[nameof(SignValueStatement.Concept)],
				statement.Concepts[nameof(SignValueStatement.Sign)],
				statement.Concepts[nameof(SignValueStatement.Value)]);
		}
	}
}
