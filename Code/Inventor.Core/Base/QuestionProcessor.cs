using System;
using System.Collections.Generic;

namespace Inventor.Core.Base
{
	public abstract class QuestionProcessor : IQuestionProcessor
	{
		public abstract IAnswer Process(IQuestionProcessingContext context);
	}

	public abstract class QuestionProcessor<QuestionT> : QuestionProcessor
		where QuestionT : IQuestion
	{
		public abstract IAnswer Process(IQuestionProcessingContext<QuestionT> context);

		public override IAnswer Process(IQuestionProcessingContext context)
		{
			return Process((QuestionProcessingContext<QuestionT>) context);
		}
	}

	public abstract class QuestionProcessor<QuestionT, StatementT> : QuestionProcessor<QuestionT>
		where QuestionT : IQuestion
		where StatementT : IStatement
	{
		protected abstract IAnswer CreateAnswer(IQuestionProcessingContext<QuestionT> context, ICollection<StatementT> statements);

		protected abstract Boolean DoesStatementMatch(IQuestionProcessingContext<QuestionT> context, StatementT statement);
	}
}