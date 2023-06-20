using System.Collections.Generic;
using System.Linq;

namespace AabSemantics.Answers
{
	public class StatementsAnswer : Answer, IAnswer<ICollection<IStatement>>
	{
		#region Properties

		public ICollection<IStatement> Result
		{ get; }

		#endregion

		public StatementsAnswer(ICollection<IStatement> result, IText description, IExplanation explanation)
			: base(description, explanation, result.Count == 0)
		{
			Result = result;
		}

		public StatementsAnswer<StatementT> MakeExplicit<StatementT>()
			where StatementT : IStatement
		{
			return new StatementsAnswer<StatementT>(
				Result.OfType<StatementT>().ToList(),
				Description,
				Explanation);
		}
	}

	public class StatementsAnswer<StatementT> : Answer, IAnswer<ICollection<StatementT>>
		where StatementT : IStatement
	{
		#region Properties

		public ICollection<StatementT> Result
		{ get; }

		#endregion

		public StatementsAnswer(ICollection<StatementT> result, IText description, IExplanation explanation)
			: base(description, explanation, result.Count == 0)
		{
			Result = result;
		}

		public StatementsAnswer MakeGeneric()
		{
			return new StatementsAnswer(
				Result.OfType<IStatement>().ToList(),
				Description,
				Explanation);
		}
	}
}
