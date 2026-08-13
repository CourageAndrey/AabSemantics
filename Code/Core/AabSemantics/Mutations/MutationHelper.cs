using System;
using System.Collections.Generic;
using System.Linq;

using AabSemantics.Utils;

namespace AabSemantics.Mutations
{
	/// <summary>Drives inference by applying mutations until none of them fires any more.</summary>
	public static class MutationHelper
	{
		/// <summary>
		/// Repeatedly applies the first mutation that fires, until a whole pass finds none —
		/// that is, until the network reaches a fixed point.
		/// </summary>
		/// <param name="semanticNetwork">Network to grow.</param>
		/// <param name="mutations">Rules to apply, tried in order.</param>
		/// <param name="updateMutationsCollections">
		/// Recomputes the rule set after each successful application, e.g. to drop one-shot rules.
		/// Keeps the set unchanged when <c>null</c>.
		/// </param>
		/// <returns>The mutations that fired, in the order they were applied; the same rule may appear repeatedly.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="semanticNetwork"/> or <paramref name="mutations"/> is <c>null</c>.</exception>
		public static ICollection<IMutation> Mutate(
			this ISemanticNetwork semanticNetwork,
			ICollection<IMutation> mutations,
			Func<ISemanticNetwork, ICollection<IMutation>, IMutation, ICollection<IMutation>> updateMutationsCollections = null)
		{
			semanticNetwork.EnsureNotNull(nameof(semanticNetwork));
			mutations.EnsureNotNull(nameof(mutations));
			if (updateMutationsCollections == null)
			{
				updateMutationsCollections = keepMutationsUnchanged;
			}

			var appliedMutations = new List<IMutation>();
			while (mutations.Count > 0)
			{
				var applied = mutations.FirstOrDefault(m => m.TryToApply(semanticNetwork));

				if (applied != null)
				{
					appliedMutations.Add(applied);
					mutations = updateMutationsCollections(semanticNetwork, mutations, applied);
				}
				else
				{
					mutations = Array.Empty<IMutation>();
				}
			}

			return appliedMutations;
		}

		private static ICollection<IMutation> keepMutationsUnchanged(ISemanticNetwork semanticNetwork, ICollection<IMutation> mutations, IMutation lastAppliedMutation)
		{
			return mutations;
		}
	}
}
