using System.Collections.Generic;

namespace AabSemantics
{
	public interface IQuestion
	{
		ICollection<IStatement> Preconditions
		{ get; }

		IAnswer Ask(ISemanticNetworkContext context, ILanguage language = null);
	}
}
