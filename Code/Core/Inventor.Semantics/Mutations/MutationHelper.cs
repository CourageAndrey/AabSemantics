using System;
using System.Collections.Generic;
using System.Linq;

using Inventor.Semantics.Utils;

namespace Inventor.Semantics.Mutations
{
	public static class MutationHelper
	{
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
