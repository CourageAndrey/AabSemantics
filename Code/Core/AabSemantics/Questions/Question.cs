using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AabSemantics.Questions
{
	public abstract class Question : IQuestion
	{
		#region Properties

		public ICollection<IStatement> Preconditions
		{ get; }

		#endregion

		protected Question(IEnumerable<IStatement> preconditions = null)
		{
			Preconditions = new List<IStatement>(preconditions ?? Array.Empty<IStatement>());
		}

		public async Task<IAnswer> AskAsync(ISemanticNetworkContext context, ILanguage language = null)
		{
			using (var questionContext = context.CreateQuestionContext(this, language))
			{
				return await ProcessAsync(questionContext).ConfigureAwait(false);
			}
		}

		public abstract Task<IAnswer> ProcessAsync(IQuestionProcessingContext context);
	}

	public static class QuestionProcessingExtensions
	{
		public static StatementQuestionProcessor<QuestionT, StatementT> From<QuestionT, StatementT>(this IQuestionProcessingContext context)
			where QuestionT : IQuestion
			where StatementT : class, IStatement
		{
			return new StatementQuestionProcessor<QuestionT, StatementT>(context);
		}
	}
}
