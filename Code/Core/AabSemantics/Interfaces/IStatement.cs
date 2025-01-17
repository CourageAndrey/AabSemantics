using System;
using System.Collections.Generic;
using System.Linq;

using AabSemantics.Metadata;
using AabSemantics.Statements;

namespace AabSemantics
{
	public interface IStatement : IKnowledge
	{
		IContext Context
		{ get; set; }

		IEnumerable<IConcept> GetChildConcepts();

		Boolean CheckUnique(IEnumerable<IStatement> statements);
	}

	public static class StatementExtensions
	{
		private static StatementDefinition GetDefinition(this IStatement statement)
		{
			var customStatement = statement as CustomStatement;
			return customStatement != null
				? Repositories.CustomStatements[customStatement.Type]
				: Repositories.Statements.Definitions[statement.GetType()];
		}

		public static IText DescribeTrue(this IStatement statement)
		{
			return GetDefinition(statement).DescribeTrue(statement);
		}

		public static IText DescribeFalse(this IStatement statement)
		{
			return GetDefinition(statement).DescribeFalse(statement);
		}

		public static IText DescribeQuestion(this IStatement statement)
		{
			return GetDefinition(statement).DescribeQuestion(statement);
		}
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
