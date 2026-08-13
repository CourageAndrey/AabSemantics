using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AabSemantics.Questions
{
	/// <summary>
	/// Base <see cref="IQuestion"/> implementation. It owns the disposable question context, so
	/// subclasses only implement <see cref="ProcessAsync"/> and need not worry about withdrawing
	/// preconditions afterwards.
	/// </summary>
	public abstract class Question : IQuestion
	{
		#region Properties

		/// <summary>Statements assumed true while answering, discarded once the answer is produced.</summary>
		public ICollection<IStatement> Preconditions
		{ get; }

		#endregion

		/// <summary>Initializes the question.</summary>
		/// <param name="preconditions">Hypothetical statements; copied into the question. <c>null</c> means none.</param>
		protected Question(IEnumerable<IStatement> preconditions = null)
		{
			Preconditions = new List<IStatement>(preconditions ?? Array.Empty<IStatement>());
		}

		/// <summary>Creates a question context, answers within it, and disposes it afterwards.</summary>
		/// <param name="context">Context to search.</param>
		/// <param name="language">Language for the answer's text; defaults to the context's language when <c>null</c>.</param>
		/// <returns>The answer.</returns>
		public async Task<IAnswer> AskAsync(ISemanticNetworkContext context, ILanguage language = null)
		{
			using (var questionContext = context.CreateQuestionContext(this, language))
			{
				return await ProcessAsync(questionContext).ConfigureAwait(false);
			}
		}

		/// <summary>Derives the answer. Called with the question context already set up.</summary>
		/// <param name="context">Context holding the question and its preconditions.</param>
		/// <returns>The answer.</returns>
		public abstract Task<IAnswer> ProcessAsync(IQuestionProcessingContext context);
	}

	/// <summary>Entry point of the fluent question-processing syntax.</summary>
	public static class QuestionProcessingExtensions
	{
		/// <summary>Starts building a processor that answers from statements of one type.</summary>
		/// <typeparam name="QuestionT">Question type being answered.</typeparam>
		/// <typeparam name="StatementT">Statement type the answer is derived from.</typeparam>
		/// <param name="context">Context to search.</param>
		/// <returns>A processor the filters and answer builders are then chained onto.</returns>
		public static StatementQuestionProcessor<QuestionT, StatementT> From<QuestionT, StatementT>(this IQuestionProcessingContext context)
			where QuestionT : IQuestion
			where StatementT : class, IStatement
		{
			return new StatementQuestionProcessor<QuestionT, StatementT>(context);
		}
	}
}
