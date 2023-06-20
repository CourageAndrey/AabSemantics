using System;

namespace AabSemantics
{
	public interface IMutation
	{
		Boolean TryToApply(ISemanticNetwork semanticNetwork);
	}
}
