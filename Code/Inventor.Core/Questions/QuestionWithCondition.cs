using System;
using System.Collections.Generic;

namespace Inventor.Core.Questions
{
	public sealed class QuestionWithCondition : Question
	{
		public IQuestion Question
		{ get; }

		public QuestionWithCondition(IEnumerable<IStatement> conditions, IQuestion question)
			: base(conditions)
		{
			if (conditions == null) throw new ArgumentNullException(nameof(conditions));
			if (question == null) throw new ArgumentNullException(nameof(question));

			Question = question;
		}
	}
}
