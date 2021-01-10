using System.Collections.Generic;
using System.Linq;

namespace Inventor.Core
{
	public class Explanation : IExplanation
	{
		#region Properties

		public ICollection<Statement> Statements
		{ get; }

		#endregion

		public Explanation(IEnumerable<Statement> statements)
		{
			Statements = statements.ToArray();
		}

		public Explanation(Statement statement)
			: this(new[] { statement })
		{ }
	}
}
