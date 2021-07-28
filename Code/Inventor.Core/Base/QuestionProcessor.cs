using System;
using System.Collections.Generic;
using System.Linq;

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
		where QuestionT : IQuestion<StatementT>
		where StatementT : IStatement
	{
		public override IAnswer Process(IQuestionProcessingContext<QuestionT> context)
		{
			foreach (var statement in context.Question.Preconditions)
			{
				statement.Context = context;
				context.KnowledgeBase.Statements.Add(statement);
			}

			var statements = context.KnowledgeBase.Statements.Enumerate<StatementT>(context.ActiveContexts).Where(statement => DoesStatementMatch(context, statement)).ToList();

			if (!NeedToCheckTransitives(context, statements))
			{
				return CreateAnswer(context, statements);
			}
			else
			{
				var valuableAnswers = new List<ChildAnswer>();
				foreach (var nested in GetNestedQuestions(context))
				{
					var answer = nested.Question.Ask(context);
					if (!answer.IsEmpty)
					{
						valuableAnswers.Add(new ChildAnswer(nested.Question, answer, nested.TransitiveStatements));
					}
				}

				return ProcessChildAnswers(context, statements, valuableAnswers);
			}
		}

		protected abstract Boolean DoesStatementMatch(IQuestionProcessingContext<QuestionT> context, StatementT statement);

		protected virtual Boolean NeedToCheckTransitives(IQuestionProcessingContext<QuestionT> context, ICollection<StatementT> statements)
		{
			return statements.Count == 0;
		}

		protected abstract IAnswer CreateAnswer(IQuestionProcessingContext<QuestionT> context, ICollection<StatementT> statements);

		protected virtual IEnumerable<NestedQuestion> GetNestedQuestions(IQuestionProcessingContext<QuestionT> context)
		{
			yield break;
		}

		protected virtual IAnswer ProcessChildAnswers(IQuestionProcessingContext<QuestionT> context, ICollection<StatementT> statements, ICollection<ChildAnswer> childAnswers)
		{
			if (childAnswers.Count > 0)
			{
				var answer = childAnswers.First();
				answer.Answer.Explanation.Expand(answer.TransitiveStatements);
				return answer.Answer;
			}
			else
			{
				return Answers.Answer.CreateUnknown(context.Language);
			}
		}
	}
}