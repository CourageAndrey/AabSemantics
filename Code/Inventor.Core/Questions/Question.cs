using System;
using System.Collections.Generic;
using System.Linq;

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
			var statements = context.From<StatementT>(DoesStatementMatch);

			ICollection<ChildAnswer> childAnswers;
			if (NeedToCheckTransitives(statements))
			{
				childAnswers = new List<ChildAnswer>();
				foreach (var nested in GetNestedQuestions(context))
				{
					var answer = nested.Question.Ask(context);
					if (!answer.IsEmpty)
					{
						childAnswers.Add(new ChildAnswer(nested.Question, answer, nested.TransitiveStatements));
					}
				}
			}
			else
			{
				childAnswers = new ChildAnswer[0];
			}

			return CreateAnswer(context, statements, childAnswers);
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
		public static List<StatementT> From<StatementT>(this IKnowledgeBaseContext context, Func<StatementT, Boolean> match)
			where StatementT : IStatement
		{
			return context.KnowledgeBase.Statements
				.Enumerate<StatementT>(context.ActiveContexts)
				.Where(match)
				.ToList();
		}
	}
}
