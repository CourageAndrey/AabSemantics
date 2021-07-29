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
			var statements = context.KnowledgeBase.Statements.Enumerate<StatementT>(context.ActiveContexts).Where(statement => DoesStatementMatch(statement)).ToList();

			if (!NeedToCheckTransitives(statements))
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

		protected abstract Boolean DoesStatementMatch(StatementT statement);

		protected virtual Boolean NeedToCheckTransitives(ICollection<StatementT> statements)
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
