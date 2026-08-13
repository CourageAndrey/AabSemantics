using System;

using AabSemantics.Utils;

namespace AabSemantics.Mutations
{
	/// <summary>
	/// Production rule in its usual "if this pattern occurs, do that" form.
	/// <para>
	/// Only the first match is acted on per call. Since the surrounding loop keeps applying the
	/// rule while it reports success, the apply method must change the network so that the same
	/// match stops occurring — otherwise the loop never terminates.
	/// </para>
	/// </summary>
	public class Production : IMutation
	{
		/// <summary>Pattern searched for in the network.</summary>
		public IsomorphicSearchPattern LookupPattern
		{ get; }

		/// <summary>Action run against the first match found.</summary>
		public Action<KnowledgeStructure> ApplyMethod
		{ get; }

		/// <summary>Creates a production rule.</summary>
		/// <param name="lookupPattern">Pattern to search for.</param>
		/// <param name="applyMethod">Action to run against a match.</param>
		/// <exception cref="ArgumentNullException">Any argument is <c>null</c>.</exception>
		public Production(IsomorphicSearchPattern lookupPattern, Action<KnowledgeStructure> applyMethod)
		{
			LookupPattern = lookupPattern.EnsureNotNull(nameof(lookupPattern));
			ApplyMethod = applyMethod.EnsureNotNull(nameof(applyMethod));
		}

		/// <summary>Runs the action against the first match, if the pattern occurs at all.</summary>
		/// <param name="semanticNetwork">Network to search and modify.</param>
		/// <returns><c>true</c> if a match was found and the action ran.</returns>
		public Boolean TryToApply(ISemanticNetwork semanticNetwork)
		{
			var match = semanticNetwork.FindFirstMatch(LookupPattern);
			if (match != null)
			{
				ApplyMethod(match);
				return true;
			}
			else
			{
				return false;
			}
		}
	}
}
