using System;
using System.Collections.Generic;

namespace Inventor.Core.Questions
{
	public sealed class QuestionWithCondition : IQuestion
	{
		public ICollection<IStatement> Conditions
		{ get; }

		public IQuestion Question
		{ get; }

		public QuestionWithCondition(IEnumerable<IStatement> conditions, IQuestion question)
		{
			if (conditions == null) throw new ArgumentNullException(nameof(conditions));
			if (question == null) throw new ArgumentNullException(nameof(question));

			Conditions = new List<IStatement>(conditions);
			Question = question;
		}
	}
}
