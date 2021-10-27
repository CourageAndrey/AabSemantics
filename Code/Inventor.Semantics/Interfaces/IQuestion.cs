using System.Collections.Generic;

namespace Inventor.Semantics
{
	public interface IQuestion
	{
		ICollection<IStatement> Preconditions
		{ get; }

		IAnswer Ask(ISemanticNetworkContext context, ILanguage language = null);
	}
}
