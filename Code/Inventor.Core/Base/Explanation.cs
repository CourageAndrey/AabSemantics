using System.Collections.Generic;
using System.Linq;

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
			Statements = statements.ToArray();
		}

		public Explanation(IStatement statement)
			: this(new[] { statement })
		{ }
	}
}
