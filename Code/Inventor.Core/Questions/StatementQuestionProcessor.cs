using System;
using System.Collections.Generic;
using System.Linq;

namespace Inventor.Core.Questions
{
	public class StatementQuestionProcessor<QuestionT, StatementT>
		where QuestionT : Question<QuestionT, StatementT>
		where StatementT : IStatement
	{
		#region Properties

		public IQuestionProcessingContext<QuestionT> Context
		{ get; }

		public ICollection<StatementT> Statements
		{ get; }

		#endregion

		public StatementQuestionProcessor(IQuestionProcessingContext<QuestionT> context, Func<StatementT, Boolean> match)
		{
			Context = context;

			Statements = context.KnowledgeBase.Statements
				.Enumerate<StatementT>(context.ActiveContexts)
				.Where(match)
				.ToList();
		}
	}
}