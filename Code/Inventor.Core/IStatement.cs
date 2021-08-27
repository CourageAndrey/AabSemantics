using System;
using System.Collections.Generic;
using System.Linq;

namespace Inventor.Core
{
	public interface IStatement : IKnowledge
	{
		IContext Context
		{ get; set; }

		IEnumerable<IConcept> GetChildConcepts();

		FormattedLine DescribeTrue();

		FormattedLine DescribeFalse();

		FormattedLine DescribeQuestion();

		Boolean CheckUnique(IEnumerable<IStatement> statements);
	}

	public static class StatementsBaseHelper
	{
		public static IEnumerable<IStatement> Enumerate(this IEnumerable<IStatement> statements, Func<IContext, Boolean> contextFilter)
		{
			foreach (var statement in statements.Where(s => contextFilter(s.Context)))
			{
				yield return statement;
			}
		}

		public static IEnumerable<IStatement> Enumerate(this IEnumerable<IStatement> statements)
		{
			return statements.Enumerate(context => true);
		}

		public static IEnumerable<IStatement> Enumerate(this IEnumerable<IStatement> statements, IContext certainContext)
		{
			return statements.Enumerate(context => context == certainContext);
		}

		public static IEnumerable<IStatement> Enumerate(this IEnumerable<IStatement> statements, ICollection<IContext> validContexts)
		{
			return statements.Enumerate(context => validContexts.Contains(context));
		}

		public static IEnumerable<StatementT> Enumerate<StatementT>(this IEnumerable<IStatement> statements, Func<IContext, Boolean> contextFilter)
			where StatementT : IStatement
		{
			foreach (var statement in statements.OfType<StatementT>().Where(s => contextFilter(s.Context)))
			{
				yield return statement;
			}
		}

		public static IEnumerable<StatementT> Enumerate<StatementT>(this IEnumerable<IStatement> statements)
			where StatementT : IStatement
		{
			return statements.Enumerate<StatementT>(context => true);
		}

		public static IEnumerable<StatementT> Enumerate<StatementT>(this IEnumerable<IStatement> statements, IContext certainContext)
			where StatementT : IStatement
		{
			return statements.Enumerate<StatementT>(context => context == certainContext);
		}

		public static IEnumerable<StatementT> Enumerate<StatementT>(this IEnumerable<IStatement> statements, ICollection<IContext> validContexts)
			where StatementT : IStatement
		{
			return statements.Enumerate<StatementT>(context => validContexts.Contains(context));
		}
	}
}
