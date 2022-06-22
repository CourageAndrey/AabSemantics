using System;

namespace Inventor.Semantics
{
	public interface IMutation
	{
		Boolean TryToApply(ISemanticNetwork semanticNetwork);
	}
}
