using System;
using System.Collections.Generic;

namespace Inventor.Core.Questions
{
	public sealed class CheckStatementQuestion : Question
	{
		public IStatement Statement
		{ get; }

		public CheckStatementQuestion(IStatement statement, IEnumerable<IStatement> preconditions = null)
			: base(preconditions)
		{
			if (statement == null) throw new ArgumentNullException(nameof(statement));

			Statement = statement;
		}
	}
}
