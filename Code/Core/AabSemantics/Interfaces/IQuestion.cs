using System.Collections.Generic;
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
		/// <returns>The answer, including the explanation of how it was derived.</returns>
		Task<IAnswer> AskAsync(ISemanticNetworkContext context, ILanguage language = null);
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
		/// <returns>The answer, including the explanation of how it was derived.</returns>
		public static IAnswer Ask(this IQuestion question, ISemanticNetworkContext context, ILanguage language = null)
		{
			return TaskHelper.AwaitDetached(() => question.AskAsync(context, language));
		}
	}
}
