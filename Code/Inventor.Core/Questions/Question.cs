﻿using System;
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
