using System.Collections.Generic;

namespace Inventor.Core
{
	public interface IExplanation
	{
		ICollection<Statement> Statements
		{ get; }
	}
}
