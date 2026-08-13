using System.Collections.Generic;

namespace AabSemantics
{
	/// <summary>
	/// The proof behind an <see cref="IAnswer"/>: the statements that, taken together,
	/// justify it. An empty explanation means the answer required no evidence.
	/// </summary>
	public interface IExplanation
	{
		/// <summary>
		/// Supporting statements, in the order the inference visited them.
		/// </summary>
		ICollection<IStatement> Statements
		{ get; }
	}

	/// <summary>
	/// Default <see cref="IExplanation"/> holding a mutable list of supporting statements.
	/// </summary>
	public class Explanation : IExplanation
	{
		#region Properties

		/// <summary>
		/// Supporting statements, in the order the inference visited them.
		/// </summary>
		public ICollection<IStatement> Statements
		{ get; }

		#endregion

		/// <summary>
		/// Creates an explanation from several statements.
		/// </summary>
		/// <param name="statements">Supporting statements; copied into the new explanation.</param>
		public Explanation(IEnumerable<IStatement> statements)
		{
			Statements = new List<IStatement>(statements);
		}

		/// <summary>
		/// Creates an explanation resting on a single statement.
		/// </summary>
		/// <param name="statement">The supporting statement.</param>
		public Explanation(IStatement statement)
			: this(new List<IStatement> { statement })
		{ }
	}

	/// <summary>
	/// Helpers for accumulating evidence while an answer is being derived.
	/// </summary>
	public static class ExplanationExtensions
	{
		/// <summary>
		/// Appends more supporting statements to an existing explanation, in place.
		/// </summary>
		/// <param name="explanation">Explanation to extend.</param>
		/// <param name="statements">Statements to append; duplicates are not filtered out.</param>
		public static void Expand(this IExplanation explanation, IEnumerable<IStatement> statements)
		{
			foreach (var statement in statements)
			{
				explanation.Statements.Add(statement);
			}
		}
	}
}
