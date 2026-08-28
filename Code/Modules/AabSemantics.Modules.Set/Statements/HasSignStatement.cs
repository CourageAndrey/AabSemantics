using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using AabSemantics.Modules.Classification.Statements;
using AabSemantics.Modules.Set.Attributes;
using AabSemantics.Modules.Set.Localization;
using AabSemantics.Statements;
using AabSemantics.Utils;

namespace AabSemantics.Modules.Set.Statements
{
	/// <summary>States that a concept has a given sign, which its descendants inherit.</summary>
	public class HasSignStatement : Statement<HasSignStatement>
	{
		#region Properties

		/// <summary>The concept in question.</summary>
		public IConcept Concept
		{ get; private set; }

		/// <summary>The sign concept.</summary>
		public IConcept Sign
		{ get; private set; }

		#endregion

		/// <summary>Creates the statement.</summary>
		/// <param name="id">Identifier; a GUID is generated when null or empty.</param>
		/// <param name="concept">The concept in question.</param>
		/// <param name="sign">The sign concept.</param>
		/// <exception cref="System.ArgumentNullException">A required concept is <c>null</c>.</exception>
		public HasSignStatement(String id, IConcept concept, IConcept sign)
			: base(
				id,
				new Func<ILanguage, String>(language => language.GetStatementsExtension<ILanguageSetModule, ILanguageStatements>().Names.HasSign),
				new Func<ILanguage, String>(language => language.GetStatementsExtension<ILanguageSetModule, ILanguageStatements>().Hints.HasSign))
		{
			Update(id, concept, sign);
		}

		/// <summary>Reassigns the identifier and the related concepts.</summary>
		/// <param name="id">New identifier; a GUID is generated when null or empty.</param>
		/// <param name="concept">The concept in question.</param>
		/// <param name="sign">The sign concept.</param>
		/// <exception cref="System.ArgumentNullException">A required concept is <c>null</c>.</exception>
		public void Update(String id, IConcept concept, IConcept sign)
		{
			Update(id);
			Concept = concept.EnsureNotNull(nameof(concept));
			Sign = sign.EnsureNotNull(nameof(sign)).EnsureHasAttribute<IConcept, IsSignAttribute>(nameof(sign));
		}

		/// <summary>Returns the concepts the statement relates.</summary>
		/// <returns>The participating concepts.</returns>
		public override IEnumerable<IConcept> GetChildConcepts()
		{
			yield return Concept;
			yield return Sign;
		}

		#region Consistency checking

		/// <summary>Compares the related concepts by reference.</summary>
		/// <param name="other">Statement to compare with; may be <c>null</c>.</param>
		/// <returns><c>true</c> if both relate the same concepts.</returns>
		public override System.Boolean Equals(HasSignStatement other)
		{
			if (ReferenceEquals(this, other)) return true;
			if (other != null)
			{
				return	other.Concept == Concept &&
						other.Sign == Sign;
			}
			else return false;
		}

		/// <summary>Reports whether this statement's sign is also declared by an ancestor of its concept.</summary>
		/// <param name="hasSigns">Sign declarations to inspect.</param>
		/// <param name="classifications">Classification statements defining the hierarchy.</param>
		/// <param name="cancellationToken">Cancels the search, which walks the whole hierarchy above the concept.</param>
		/// <returns><c>true</c> when the sign is declared more than once along the chain.</returns>
		/// <exception cref="OperationCanceledException">The token was cancelled.</exception>
		public System.Boolean CheckSignDuplication(IEnumerable<HasSignStatement> hasSigns, IEnumerable<IsStatement> classifications, CancellationToken cancellationToken = default)
		{
			var signs = hasSigns.Where(hs => hs.Concept == Concept).Select(hs => hs.Sign).Observing(cancellationToken).ToList();
			foreach (var parent in classifications.GetParentsAllLevels(Concept, cancellationToken: cancellationToken))
			{
				foreach (var parentSign in hasSigns.Where(hs => hs.Concept == parent).Select(hs => hs.Sign))
				{
					if (signs.Contains(parentSign))
					{
						return false;
					}
				}
			}

			return true;
		}

		/// <summary>Asynchronous counterpart of <see cref="CheckSignDuplication"/>.</summary>
		/// <param name="hasSigns">Sign declarations to inspect.</param>
		/// <param name="classifications">Classification statements defining the hierarchy.</param>
		/// <param name="cancellationToken">Cancels the search.</param>
		/// <returns><c>true</c> when the sign is declared more than once along the chain.</returns>
		/// <exception cref="OperationCanceledException">The token was cancelled.</exception>
		public Task<System.Boolean> CheckSignDuplicationAsync(IEnumerable<HasSignStatement> hasSigns, IEnumerable<IsStatement> classifications, CancellationToken cancellationToken = default)
		{
			return TaskHelper.FromSynchronous(() => CheckSignDuplication(hasSigns, classifications, cancellationToken), cancellationToken);
		}

		#endregion

		/// <summary>Collects the signs declared for a concept.</summary>
		/// <param name="statements">Statements to search.</param>
		/// <param name="concept">Concept whose signs are wanted.</param>
		/// <param name="recursive">When <c>true</c>, signs inherited from ancestors are included.</param>
		/// <param name="cancellationToken">Cancels the search; a recursive one walks the whole hierarchy above the concept.</param>
		/// <returns>The matching sign declarations.</returns>
		/// <exception cref="OperationCanceledException">The token was cancelled.</exception>
		public static List<HasSignStatement> GetSigns(IEnumerable<IStatement> statements, IConcept concept, System.Boolean recursive, CancellationToken cancellationToken = default)
		{
			// the statements are filtered once here rather than at every level of the hierarchy
			var hasSigns = statements.OfType<HasSignStatement>().Observing(cancellationToken).ToList();

			var result = new List<HasSignStatement>(hasSigns.Where(sv => sv.Concept == concept));

			if (recursive)
			{
				var classifications = statements.OfType<IsStatement>().Observing(cancellationToken).ToList();
				InheritSigns(result, hasSigns, classifications, concept, cancellationToken);
			}

			return result;
		}

		/// <summary>Asynchronous counterpart of <see cref="GetSigns"/>.</summary>
		/// <param name="statements">Statements to search.</param>
		/// <param name="concept">Concept whose signs are wanted.</param>
		/// <param name="recursive">When <c>true</c>, signs inherited from ancestors are included.</param>
		/// <param name="cancellationToken">Cancels the search.</param>
		/// <returns>The matching sign declarations.</returns>
		/// <exception cref="OperationCanceledException">The token was cancelled.</exception>
		public static Task<List<HasSignStatement>> GetSignsAsync(IEnumerable<IStatement> statements, IConcept concept, System.Boolean recursive, CancellationToken cancellationToken = default)
		{
			return TaskHelper.FromSynchronous(() => GetSigns(statements, concept, recursive, cancellationToken), cancellationToken);
		}

		private static void InheritSigns(
			List<HasSignStatement> result,
			ICollection<HasSignStatement> hasSigns,
			ICollection<IsStatement> classifications,
			IConcept concept,
			CancellationToken cancellationToken)
		{
			foreach (var parent in classifications.GetParentsOneLevel(concept, cancellationToken: cancellationToken))
			{
				result.AddRange(hasSigns.Where(sv => sv.Concept == parent));

				InheritSigns(result, hasSigns, classifications, parent, cancellationToken);
			}
		}
	}
}
