using System;

namespace AabSemantics
{
	/// <summary>
	/// A production rule: a transformation that derives new knowledge from what a semantic
	/// network already contains. Mutations are applied repeatedly until none of them changes
	/// anything, which is how the engine reaches a fixed point.
	/// </summary>
	public interface IMutation
	{
		/// <summary>
		/// Applies the rule to the network if its preconditions hold.
		/// </summary>
		/// <param name="semanticNetwork">Network to inspect and, on success, modify in place.</param>
		/// <returns>
		/// <c>true</c> if the network was changed. Returning <c>true</c> without changing anything
		/// would prevent the surrounding loop from ever terminating.
		/// </returns>
		Boolean TryToApply(ISemanticNetwork semanticNetwork);
	}
}
