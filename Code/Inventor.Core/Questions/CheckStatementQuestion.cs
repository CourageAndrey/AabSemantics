using System;

namespace Inventor.Core.Questions
{
	public sealed class CheckStatementQuestion : IQuestion
	{
		public IStatement Statement
		{ get; }

		public CheckStatementQuestion(IStatement statement)
		{
			if (statement == null) throw new ArgumentNullException(nameof(statement));

			Statement = statement;
		}
	}
}
