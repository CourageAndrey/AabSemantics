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
	}

	public abstract class Question<QuestionT> : Question
		where QuestionT : Question<QuestionT>
	{
		protected Question(IEnumerable<IStatement> preconditions = null)
			: base(preconditions)
		{
		}
	}
}
