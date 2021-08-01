using System.Collections.Generic;

namespace Inventor.Core
{
	public interface IQuestion
	{
		ICollection<IStatement> Preconditions
		{ get; }

		IAnswer Ask(ISemanticNetworkContext context);
	}
}
