using System.Collections.Generic;

namespace Inventor.Core.Questions
{
	public abstract class Question : IQuestion
	{
		public ICollection<IStatement> Preconditions
		{ get; }

		protected Question(IEnumerable<IStatement> preconditions = null)
		{
			Preconditions = new List<IStatement>(preconditions ?? new IStatement[0]);
		}
	}
}
