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
			Preconditions = new List<IStatement>(preconditions ?? new IStatement[0]);
		}

		public IAnswer Ask(IKnowledgeBaseContext context)
		{
			using (var questionContext = context.CreateQuestionContext(this))
			{
				return Process(questionContext);
			}
		}

		public abstract IAnswer Process(IQuestionProcessingContext context);
	}

	public abstract class Question<QuestionT> : Question
		where QuestionT : Question<QuestionT>
	{
		protected Question(IEnumerable<IStatement> preconditions = null)
			: base(preconditions)
		{
		}

		public override IAnswer Process(IQuestionProcessingContext context)
		{
			return Process((IQuestionProcessingContext<QuestionT>) context);
		}

		public abstract IAnswer Process(IQuestionProcessingContext<QuestionT> context);
	}

	public abstract class Question<QuestionT, StatementT> : Question<QuestionT>, IQuestion<StatementT>
		where QuestionT : Question<QuestionT, StatementT>
		where StatementT : IStatement
	{
		protected Question(IEnumerable<IStatement> preconditions = null)
			: base(preconditions)
		{
		}

		public override IAnswer Process(IQuestionProcessingContext<QuestionT> context)
		{
			return context
				.From<QuestionT, StatementT>(DoesStatementMatch)
				.ProcessTransitives(NeedToCheckTransitives, GetNestedQuestions)
				.Select(CreateAnswer);
		}

		protected abstract Boolean DoesStatementMatch(StatementT statement);

		protected abstract Boolean NeedToCheckTransitives(ICollection<StatementT> statements);

		protected abstract IAnswer CreateAnswer(IQuestionProcessingContext<QuestionT> context, ICollection<StatementT> statements, ICollection<ChildAnswer> childAnswers);

		protected virtual IEnumerable<NestedQuestion> GetNestedQuestions(IQuestionProcessingContext<QuestionT> context)
		{
			yield break;
		}

		protected abstract IAnswer ProcessChildAnswers(IQuestionProcessingContext<QuestionT> context, ICollection<StatementT> statements, ICollection<ChildAnswer> childAnswers);
	}

	public static class QuestionProcessingExtensions
	{
		public static StatementQuestionProcessor<QuestionT, StatementT> From<QuestionT, StatementT>(this IQuestionProcessingContext context, Func<StatementT, Boolean> match)
			where QuestionT : IQuestion
			where StatementT : IStatement
		{
			return new StatementQuestionProcessor<QuestionT, StatementT>(context, match);
		}
	}
}
