using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using AabSemantics.Utils;

namespace AabSemantics
{
	/// <summary>
	/// A query against a semantic network. Implementations describe what is being asked;
	/// the matching question processor derives the answer from the network's statements.
	/// </summary>
	public interface IQuestion
	{
		/// <summary>
		/// Extra statements assumed to be true while answering, without being stored in the network.
		/// Use them to ask hypothetical questions ("if A were a B, then...").
		/// </summary>
		ICollection<IStatement> Preconditions
		{ get; }

		/// <summary>
		/// Answers the question against the given context.
		/// </summary>
		/// <param name="context">Context to search; its statements and those of its parents are considered.</param>
		/// <param name="language">Language for the answer's text. Defaults to the context's language when <c>null</c>.</param>
		/// <param name="cancellationToken">
		/// Cancels the inference. Answering a question is unbounded work — it filters the whole
		/// knowledge base, walks concept hierarchies and re-asks itself of related concepts — so a
		/// caller that can go away, such as an HTTP request or a user interface, should supply a token.
		/// </param>
		/// <returns>The answer, including the explanation of how it was derived.</returns>
		/// <exception cref="OperationCanceledException">The token was cancelled before the answer was ready.</exception>
		Task<IAnswer> AskAsync(ISemanticNetworkContext context, ILanguage language = null, CancellationToken cancellationToken = default);
	}

	/// <summary>
	/// Synchronous convenience wrappers over <see cref="IQuestion"/>.
	/// </summary>
	public static class QuestionExtensions
	{
		/// <summary>
		/// Blocking counterpart of <see cref="IQuestion.AskAsync"/>, for callers that cannot await.
		/// </summary>
		/// <param name="question">Question to ask.</param>
		/// <param name="context">Context to search.</param>
		/// <param name="language">Language for the answer's text. Defaults to the context's language when <c>null</c>.</param>
		/// <param name="cancellationToken">
		/// Cancels the inference. The calling thread stays blocked until the inference actually
		/// unwinds, but the work itself is abandoned.
		/// </param>
		/// <returns>The answer, including the explanation of how it was derived.</returns>
		/// <exception cref="OperationCanceledException">The token was cancelled before the answer was ready.</exception>
		public static IAnswer Ask(this IQuestion question, ISemanticNetworkContext context, ILanguage language = null, CancellationToken cancellationToken = default)
		{
			return TaskHelper.AwaitDetached(() => question.AskAsync(context, language, cancellationToken));
		}
	}
}
