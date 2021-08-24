using System;
using System.Collections.Generic;

namespace Inventor.Core.Questions
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

		public IAnswer Ask(ISemanticNetworkContext context, ILanguage language = null)
		{
			using (var questionContext = context.CreateQuestionContext(this, language))
			{
				return Process(questionContext);
			}
		}

		public abstract IAnswer Process(IQuestionProcessingContext context);
	}

	public static class QuestionProcessingExtensions
	{
		public static StatementQuestionProcessor<QuestionT, StatementT> From<QuestionT, StatementT>(this IQuestionProcessingContext context)
			where QuestionT : IQuestion
			where StatementT : IStatement
		{
			return new StatementQuestionProcessor<QuestionT, StatementT>(context);
		}
	}
}
