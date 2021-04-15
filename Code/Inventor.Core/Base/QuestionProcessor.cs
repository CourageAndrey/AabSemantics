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
		where QuestionT : IQuestion
		where StatementT : IStatement
	{
		public override IAnswer Process(IQuestionProcessingContext<QuestionT> context)
		{
			foreach (var statement in GetPreconditions(context))
			{
				statement.Context = context;
				context.KnowledgeBase.Statements.Add(statement);
			}

			var activeContexts = context.GetHierarchy();
			var statements = context.KnowledgeBase.Statements.Enumerate<StatementT>(activeContexts).Where(statement => DoesStatementMatch(context, statement)).ToList();

			if (AreEnoughToAnswer(context, statements))
			{
				return CreateAnswer(context, statements);
			}

			IDictionary<IAnswer, ICollection<IStatement>> valuableAnswers = new Dictionary<IAnswer, ICollection<IStatement>>();
			foreach (var nested in GetNestedQuestions(context))
			{
				var answer = nested.Question.Ask(context);
				if (!answer.IsEmpty)
				{
					valuableAnswers.Add(answer, nested.TransitiveStatements);
				}
			}

			return ProcessChildAnswers(context, statements, valuableAnswers);
		}

		protected virtual IEnumerable<IStatement> GetPreconditions(IQuestionProcessingContext<QuestionT> context)
		{
			yield break;
		}

		protected abstract Boolean DoesStatementMatch(IQuestionProcessingContext<QuestionT> context, StatementT statement);

		protected virtual Boolean AreEnoughToAnswer(IQuestionProcessingContext<QuestionT> context, ICollection<StatementT> statements)
		{
			return statements.Count > 0;
		}

		protected abstract IAnswer CreateAnswer(IQuestionProcessingContext<QuestionT> context, ICollection<StatementT> statements);

		protected virtual IEnumerable<NestedQuestion> GetNestedQuestions(IQuestionProcessingContext<QuestionT> context)
		{
			yield break;
		}

		protected virtual IAnswer ProcessChildAnswers(IQuestionProcessingContext<QuestionT> context, ICollection<StatementT> statements, IDictionary<IAnswer, ICollection<IStatement>> childAnswers)
		{
			if (childAnswers.Count > 0)
			{
				var answer = childAnswers.First();
				answer.Key.Explanation.Expand(answer.Value);
				return answer.Key;
			}
			else
			{
				return Answers.Answer.CreateUnknown(context.Language);
			}
		}
	}

	public class NestedQuestion
	{
		public IQuestion Question
		{ get; }

		public ICollection<IStatement> TransitiveStatements
		{ get; }

		public NestedQuestion(IQuestion question, ICollection<IStatement> transitiveStatements)
		{
			Question = question;
			TransitiveStatements = transitiveStatements;
		}
	}
}