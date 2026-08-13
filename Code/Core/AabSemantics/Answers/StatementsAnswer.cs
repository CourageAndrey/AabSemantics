using System.Collections.Generic;
using System.Linq;

namespace AabSemantics.Answers
{
	/// <summary>Answer listing statements. Empty when the list is empty.</summary>
	public class StatementsAnswer : Answer, IAnswer<ICollection<IStatement>>
	{
		#region Properties

		/// <summary>The statements found.</summary>
		public ICollection<IStatement> Result
		{ get; }

		#endregion

		/// <summary>Creates a statement-list answer.</summary>
		/// <param name="result">The statements found; an empty collection makes the answer empty.</param>
		/// <param name="description">The answer as localizable text.</param>
		/// <param name="explanation">Statements the answer was derived from.</param>
		public StatementsAnswer(ICollection<IStatement> result, IText description, IExplanation explanation)
			: base(description, explanation, result.Count == 0)
		{
			Result = result;
		}

		/// <summary>Narrows the answer to statements of one type, silently dropping the others.</summary>
		/// <typeparam name="StatementT">Statement type to keep.</typeparam>
		/// <returns>A typed answer with the same text and explanation.</returns>
		public StatementsAnswer<StatementT> MakeExplicit<StatementT>()
			where StatementT : IStatement
		{
			return new StatementsAnswer<StatementT>(
				Result.OfType<StatementT>().ToList(),
				Description,
				Explanation);
		}
	}

	/// <summary>Answer listing statements of one type. Empty when the list is empty.</summary>
	/// <typeparam name="StatementT">Statement type listed.</typeparam>
	public class StatementsAnswer<StatementT> : Answer, IAnswer<ICollection<StatementT>>
		where StatementT : IStatement
	{
		#region Properties

		/// <summary>The statements found.</summary>
		public ICollection<StatementT> Result
		{ get; }

		#endregion

		/// <summary>Creates a typed statement-list answer.</summary>
		/// <param name="result">The statements found; an empty collection makes the answer empty.</param>
		/// <param name="description">The answer as localizable text.</param>
		/// <param name="explanation">Statements the answer was derived from.</param>
		public StatementsAnswer(ICollection<StatementT> result, IText description, IExplanation explanation)
			: base(description, explanation, result.Count == 0)
		{
			Result = result;
		}

		/// <summary>Widens the answer back to the untyped statement list.</summary>
		/// <returns>An untyped answer with the same text and explanation.</returns>
		public StatementsAnswer MakeGeneric()
		{
			return new StatementsAnswer(
				Result.OfType<IStatement>().ToList(),
				Description,
				Explanation);
		}
	}
}
