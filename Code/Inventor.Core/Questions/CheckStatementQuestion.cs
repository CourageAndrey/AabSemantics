using System;
using System.Collections.Generic;

namespace Inventor.Core.Questions
{
	public sealed class CheckStatementQuestion : Question<CheckStatementQuestion>
	{
		#region Properties

		public IStatement Statement
		{ get; }

		#endregion

		public CheckStatementQuestion(IStatement statement, IEnumerable<IStatement> preconditions = null)
			: base(preconditions)
		{
			if (statement == null) throw new ArgumentNullException(nameof(statement));

			Statement = statement;
		}
	}
}
