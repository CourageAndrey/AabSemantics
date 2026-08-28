using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using AabSemantics.Modules.Boolean.Attributes;
using AabSemantics.Modules.Classification.Statements;
using AabSemantics.Modules.Set.Attributes;
using AabSemantics.Modules.Set.Localization;
using AabSemantics.Statements;
using AabSemantics.Utils;

namespace AabSemantics.Modules.Set.Statements
{
	/// <summary>States the value a concept's sign takes.</summary>
	public class SignValueStatement : Statement<SignValueStatement>
	{
		#region Properties

		/// <summary>The concept in question.</summary>
		public IConcept Concept
		{ get; private set; }

		/// <summary>The sign concept.</summary>
		public IConcept Sign
		{ get; private set; }

		/// <summary>The value concept.</summary>
		public IConcept Value
		{ get; private set; }

		#endregion

		/// <summary>Creates the statement.</summary>
		/// <param name="id">Identifier; a GUID is generated when null or empty.</param>
		/// <param name="concept">The concept in question.</param>
		/// <param name="sign">The sign concept.</param>
		/// <param name="value">The value concept.</param>
		/// <exception cref="System.ArgumentNullException">A required concept is <c>null</c>.</exception>
		public SignValueStatement(String id, IConcept concept, IConcept sign, IConcept value)
			: base(
				id,
				new Func<ILanguage, String>(language => language.GetStatementsExtension<ILanguageSetModule, ILanguageStatements>().Names.SignValue),
				new Func<ILanguage, String>(language => language.GetStatementsExtension<ILanguageSetModule, ILanguageStatements>().Hints.SignValue))
		{
			Update(id, concept, sign, value);
		}

		/// <summary>Reassigns the identifier and the related concepts.</summary>
		/// <param name="id">New identifier; a GUID is generated when null or empty.</param>
		/// <param name="concept">The concept in question.</param>
		/// <param name="sign">The sign concept.</param>
		/// <param name="value">The value concept.</param>
		/// <exception cref="System.ArgumentNullException">A required concept is <c>null</c>.</exception>
		public void Update(String id, IConcept concept, IConcept sign, IConcept value)
		{
			Update(id);
			Concept = concept.EnsureNotNull(nameof(concept));
			Sign = sign.EnsureNotNull(nameof(sign)).EnsureHasAttribute<IConcept, IsSignAttribute>(nameof(sign));
			Value = value.EnsureNotNull(nameof(value)).EnsureHasAttribute<IConcept, IsValueAttribute>(nameof(value));
		}

		/// <summary>Returns the concepts the statement relates.</summary>
		/// <returns>The participating concepts.</returns>
		public override IEnumerable<IConcept> GetChildConcepts()
		{
			yield return Concept;
			yield return Sign;
			yield return Value;
		}

		#region Consistency checking

		/// <summary>Compares the related concepts by reference.</summary>
		/// <param name="other">Statement to compare with; may be <c>null</c>.</param>
		/// <returns><c>true</c> if both relate the same concepts.</returns>
		public override System.Boolean Equals(SignValueStatement other)
		{
			if (ReferenceEquals(this, other)) return true;
			if (other != null)
			{
				return	other.Concept == Concept &&
						other.Sign == Sign &&
						other.Value == Value;
			}
			else return false;
		}

		/// <summary>Verifies that the sign this statement assigns a value to is actually declared for the concept.</summary>
		/// <param name="statements">Statements to search for the sign declaration.</param>
		/// <param name="cancellationToken">Cancels the search, which walks the whole hierarchy above the concept.</param>
		/// <returns><c>true</c> when a matching declaration exists.</returns>
		/// <exception cref="OperationCanceledException">The token was cancelled.</exception>
		public System.Boolean CheckHasSign(IEnumerable<IStatement> statements, CancellationToken cancellationToken = default)
		{
			var signs = HasSignStatement.GetSigns(statements, Concept, true, cancellationToken);
			return signs.Select(hs => hs.Sign).Observing(cancellationToken).Contains(Sign);
		}

		/// <summary>Asynchronous counterpart of <see cref="CheckHasSign"/>.</summary>
		/// <param name="statements">Statements to search for the sign declaration.</param>
		/// <param name="cancellationToken">Cancels the search.</param>
		/// <returns><c>true</c> when a matching declaration exists.</returns>
		/// <exception cref="OperationCanceledException">The token was cancelled.</exception>
		public Task<System.Boolean> CheckHasSignAsync(IEnumerable<IStatement> statements, CancellationToken cancellationToken = default)
		{
			return TaskHelper.FromSynchronous(() => CheckHasSign(statements, cancellationToken), cancellationToken);
		}

		#endregion

		/// <summary>Finds the value of one sign for a concept, following the classification hierarchy upwards.</summary>
		/// <param name="statements">Statements to search.</param>
		/// <param name="concept">Concept whose sign value is wanted.</param>
		/// <param name="sign">Sign whose value is wanted.</param>
		/// <param name="cancellationToken">Cancels the search, which walks the whole hierarchy above the concept.</param>
		/// <returns>The matching statement, or <c>null</c> when the value is undefined.</returns>
		/// <exception cref="OperationCanceledException">The token was cancelled.</exception>
		public static SignValueStatement GetSignValue(IEnumerable<IStatement> statements, IConcept concept, IConcept sign, CancellationToken cancellationToken = default)
		{
			// the statements are filtered once here rather than at every level of the hierarchy
			var signValues = statements.OfType<SignValueStatement>().Observing(cancellationToken).ToList();
			var classifications = statements.OfType<IsStatement>().Observing(cancellationToken).ToList();

			return FindSignValue(signValues, classifications, concept, sign, cancellationToken);
		}

		/// <summary>Asynchronous counterpart of <see cref="GetSignValue"/>.</summary>
		/// <param name="statements">Statements to search.</param>
		/// <param name="concept">Concept whose sign value is wanted.</param>
		/// <param name="sign">Sign whose value is wanted.</param>
		/// <param name="cancellationToken">Cancels the search.</param>
		/// <returns>The matching statement, or <c>null</c> when the value is undefined.</returns>
		/// <exception cref="OperationCanceledException">The token was cancelled.</exception>
		public static Task<SignValueStatement> GetSignValueAsync(IEnumerable<IStatement> statements, IConcept concept, IConcept sign, CancellationToken cancellationToken = default)
		{
			return TaskHelper.FromSynchronous(() => GetSignValue(statements, concept, sign, cancellationToken), cancellationToken);
		}

		private static SignValueStatement FindSignValue(
			ICollection<SignValueStatement> signValues,
			ICollection<IsStatement> classifications,
			IConcept concept,
			IConcept sign,
			CancellationToken cancellationToken)
		{
			var signValue = signValues.Observing(cancellationToken).FirstOrDefault(sv => sv.Concept == concept && sv.Sign == sign);
			if (signValue != null)
			{
				return signValue;
			}

			foreach (var parent in classifications.GetParentsOneLevel(concept, cancellationToken: cancellationToken))
			{
				var parentValue = FindSignValue(signValues, classifications, parent, sign, cancellationToken);
				if (parentValue != null)
				{
					return parentValue;
				}
			}

			return null;
		}

		/// <summary>Collects the sign values defined for a concept.</summary>
		/// <param name="statements">Statements to search.</param>
		/// <param name="concept">Concept whose sign values are wanted.</param>
		/// <param name="recursive">When <c>true</c>, values inherited from ancestors are included.</param>
		/// <param name="cancellationToken">Cancels the search; a recursive one walks the whole hierarchy above the concept.</param>
		/// <returns>The matching sign value statements.</returns>
		/// <exception cref="OperationCanceledException">The token was cancelled.</exception>
		public static List<SignValueStatement> GetSignValues(IEnumerable<IStatement> statements, IConcept concept, System.Boolean recursive, CancellationToken cancellationToken = default)
		{
			// the statements are filtered once here rather than at every level of the hierarchy
			var signValues = statements.OfType<SignValueStatement>().Observing(cancellationToken).ToList();

			var result = new List<SignValueStatement>(signValues.Where(sv => sv.Concept == concept));

			if (recursive)
			{
				var classifications = statements.OfType<IsStatement>().Observing(cancellationToken).ToList();
				InheritSignValues(result, signValues, classifications, concept, cancellationToken);
			}

			return result;
		}

		/// <summary>Asynchronous counterpart of <see cref="GetSignValues"/>.</summary>
		/// <param name="statements">Statements to search.</param>
		/// <param name="concept">Concept whose sign values are wanted.</param>
		/// <param name="recursive">When <c>true</c>, values inherited from ancestors are included.</param>
		/// <param name="cancellationToken">Cancels the search.</param>
		/// <returns>The matching sign value statements.</returns>
		/// <exception cref="OperationCanceledException">The token was cancelled.</exception>
		public static Task<List<SignValueStatement>> GetSignValuesAsync(IEnumerable<IStatement> statements, IConcept concept, System.Boolean recursive, CancellationToken cancellationToken = default)
		{
			return TaskHelper.FromSynchronous(() => GetSignValues(statements, concept, recursive, cancellationToken), cancellationToken);
		}

		private static void InheritSignValues(
			List<SignValueStatement> result,
			ICollection<SignValueStatement> signValues,
			ICollection<IsStatement> classifications,
			IConcept concept,
			CancellationToken cancellationToken)
		{
			foreach (var parent in classifications.GetParentsOneLevel(concept, cancellationToken: cancellationToken))
			{
				foreach (var signValue in signValues.Where(sv => sv.Concept == parent))
				{
					// a sign already valued by the concept itself, or by a nearer ancestor, is not inherited again
					if (!result.Any(sv => sv.Sign == signValue.Sign))
					{
						result.Add(signValue);
					}
				}

				InheritSignValues(result, signValues, classifications, parent, cancellationToken);
			}
		}

	}
}
