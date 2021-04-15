using System.Collections.Generic;

namespace Inventor.Core.Base
{
	public class Explanation : IExplanation
	{
		#region Properties

		public ICollection<IStatement> Statements
		{ get; }

		#endregion

		public Explanation(IEnumerable<IStatement> statements)
		{
			Statements = new List<IStatement>(statements);
		}

		public Explanation(IStatement statement)
			: this(new List<IStatement> { statement })
		{ }
	}
}
