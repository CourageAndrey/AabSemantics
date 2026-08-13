using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AabSemantics.Metadata;
using AabSemantics.Statements;

namespace AabSemantics
{
	/// <summary>
	/// An edge of the semantic network: an assertion about one or more concepts.
	/// Statements are the facts that questions are answered from.
	/// </summary>
	public interface IStatement : IKnowledge
	{
		/// <summary>
		/// Context the statement belongs to. Assigned automatically when the statement is added
		/// to a semantic network, and used to scope inference to a subset of the knowledge base.
		/// </summary>
		IContext Context
		{ get; set; }

		/// <summary>
		/// Returns every concept the statement refers to. Removing any of them from the network
		/// also removes this statement.
		/// </summary>
		/// <returns>Concepts participating in the statement.</returns>
		IEnumerable<IConcept> GetChildConcepts();

		/// <summary>
		/// Checks that the statement does not duplicate any of the given ones.
		/// </summary>
		/// <param name="statements">Statements to compare against, typically those already in the network.</param>
		/// <returns><c>true</c> if the statement carries new information.</returns>
		Task<Boolean> CheckUniqueAsync(IEnumerable<IStatement> statements);
	}

	/// <summary>
	/// Renders a statement as human-readable text using the metadata registered for its type.
	/// </summary>
	public static class StatementExtensions
	{
		private static StatementDefinition GetDefinition(this IStatement statement)
		{
			var customStatement = statement as CustomStatement;
			return customStatement != null
				? Repositories.CustomStatements[customStatement.Type]
				: Repositories.Statements.Definitions[statement.GetType()];
		}

		/// <summary>
		/// Describes the statement as an affirmative sentence, e.g. "A is a B".
		/// </summary>
		/// <param name="statement">Statement to describe.</param>
		/// <returns>Localizable text.</returns>
		public static IText DescribeTrue(this IStatement statement)
		{
			return GetDefinition(statement).DescribeTrue(statement);
		}

		/// <summary>
		/// Describes the statement as a negative sentence, e.g. "A is not a B".
		/// </summary>
		/// <param name="statement">Statement to describe.</param>
		/// <returns>Localizable text.</returns>
		public static IText DescribeFalse(this IStatement statement)
		{
			return GetDefinition(statement).DescribeFalse(statement);
		}

		/// <summary>
		/// Describes the statement as a question, e.g. "Is A a B?".
		/// </summary>
		/// <param name="statement">Statement to describe.</param>
		/// <returns>Localizable text.</returns>
		public static IText DescribeQuestion(this IStatement statement)
		{
			return GetDefinition(statement).DescribeQuestion(statement);
		}
	}

	/// <summary>
	/// Filters statement sequences by context and by statement type.
	/// These overloads are the standard way for question processors to narrow the knowledge base.
	/// </summary>
	public static class StatementsBaseHelper
	{
		/// <summary>
		/// Selects statements whose context satisfies the given predicate.
		/// </summary>
		/// <param name="statements">Statements to filter.</param>
		/// <param name="contextFilter">Predicate applied to each statement's context.</param>
		/// <returns>Lazily evaluated matching statements.</returns>
		public static IEnumerable<IStatement> Enumerate(this IEnumerable<IStatement> statements, Predicate<IContext> contextFilter)
		{
			foreach (var statement in statements.Where(s => contextFilter(s.Context)))
			{
				yield return statement;
			}
		}

		/// <summary>
		/// Returns all statements regardless of context.
		/// </summary>
		/// <param name="statements">Statements to enumerate.</param>
		/// <returns>Lazily evaluated statements.</returns>
		public static IEnumerable<IStatement> Enumerate(this IEnumerable<IStatement> statements)
		{
			return statements.Enumerate(context => true);
		}

		/// <summary>
		/// Selects statements belonging to exactly one context.
		/// </summary>
		/// <param name="statements">Statements to filter.</param>
		/// <param name="certainContext">Context to match by reference.</param>
		/// <returns>Lazily evaluated matching statements.</returns>
		public static IEnumerable<IStatement> Enumerate(this IEnumerable<IStatement> statements, IContext certainContext)
		{
			return statements.Enumerate(context => context == certainContext);
		}

		/// <summary>
		/// Selects statements belonging to any of the given contexts.
		/// </summary>
		/// <param name="statements">Statements to filter.</param>
		/// <param name="validContexts">Accepted contexts.</param>
		/// <returns>Lazily evaluated matching statements.</returns>
		public static IEnumerable<IStatement> Enumerate(this IEnumerable<IStatement> statements, ICollection<IContext> validContexts)
		{
			return statements.Enumerate(context => validContexts.Contains(context));
		}

		/// <summary>
		/// Selects statements of the given type whose context satisfies the predicate.
		/// </summary>
		/// <typeparam name="StatementT">Statement type to keep.</typeparam>
		/// <param name="statements">Statements to filter.</param>
		/// <param name="contextFilter">Predicate applied to each statement's context.</param>
		/// <returns>Lazily evaluated matching statements.</returns>
		public static IEnumerable<StatementT> Enumerate<StatementT>(this IEnumerable<IStatement> statements, Predicate<IContext> contextFilter)
			where StatementT : IStatement
		{
			foreach (var statement in statements.OfType<StatementT>().Where(s => contextFilter(s.Context)))
			{
				yield return statement;
			}
		}

		/// <summary>
		/// Selects statements of the given type regardless of context.
		/// </summary>
		/// <typeparam name="StatementT">Statement type to keep.</typeparam>
		/// <param name="statements">Statements to filter.</param>
		/// <returns>Lazily evaluated matching statements.</returns>
		public static IEnumerable<StatementT> Enumerate<StatementT>(this IEnumerable<IStatement> statements)
			where StatementT : IStatement
		{
			return statements.Enumerate<StatementT>(context => true);
		}

		/// <summary>
		/// Selects statements of the given type belonging to exactly one context.
		/// </summary>
		/// <typeparam name="StatementT">Statement type to keep.</typeparam>
		/// <param name="statements">Statements to filter.</param>
		/// <param name="certainContext">Context to match by reference.</param>
		/// <returns>Lazily evaluated matching statements.</returns>
		public static IEnumerable<StatementT> Enumerate<StatementT>(this IEnumerable<IStatement> statements, IContext certainContext)
			where StatementT : IStatement
		{
			return statements.Enumerate<StatementT>(context => context == certainContext);
		}

		/// <summary>
		/// Selects statements of the given type belonging to any of the given contexts.
		/// </summary>
		/// <typeparam name="StatementT">Statement type to keep.</typeparam>
		/// <param name="statements">Statements to filter.</param>
		/// <param name="validContexts">Accepted contexts.</param>
		/// <returns>Lazily evaluated matching statements.</returns>
		public static IEnumerable<StatementT> Enumerate<StatementT>(this IEnumerable<IStatement> statements, ICollection<IContext> validContexts)
			where StatementT : IStatement
		{
			return statements.Enumerate<StatementT>(context => validContexts.Contains(context));
		}
	}
}
