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
		public async Task<System.Boolean> CheckHasSignAsync(IEnumerable<IStatement> statements, CancellationToken cancellationToken = default)
		{
			var signs = await HasSignStatement.GetSignsAsync(statements, Concept, true, cancellationToken).ConfigureAwait(false);
			return (await signs.Select(hs => hs.Sign).ToListAsync(cancellationToken).ConfigureAwait(false)).Contains(Sign);
		}

		#endregion

		/// <summary>Finds the value of one sign for a concept, following the classification hierarchy upwards.</summary>
		/// <param name="statements">Statements to search.</param>
		/// <param name="concept">Concept whose sign value is wanted.</param>
		/// <param name="sign">Sign whose value is wanted.</param>
		/// <param name="cancellationToken">Cancels the search, which walks the whole hierarchy above the concept.</param>
		/// <returns>The matching statement, or <c>null</c> when the value is undefined.</returns>
		/// <exception cref="OperationCanceledException">The token was cancelled.</exception>
		public static async Task<SignValueStatement> GetSignValueAsync(IEnumerable<IStatement> statements, IConcept concept, IConcept sign, CancellationToken cancellationToken = default)
		{
			var signValues = await statements.OfType<SignValueStatement>().ToListAsync(cancellationToken);
			var signValue = await signValues.FirstOrDefaultAsync(sv => sv.Concept == concept && sv.Sign == sign, cancellationToken);
			if (signValue != null)
			{
				return signValue;
			}
			else
			{
				foreach (var parent in await statements.GetParentsOneLevelAsync<IConcept, IsStatement>(concept, cancellationToken: cancellationToken))
				{
					var parentValue = await GetSignValueAsync(statements, parent, sign, cancellationToken);
					if (parentValue != null)
					{
						return parentValue;
					}
				}

				return null;
			}
		}

		/// <summary>Collects the sign values defined for a concept.</summary>
		/// <param name="statements">Statements to search.</param>
		/// <param name="concept">Concept whose sign values are wanted.</param>
		/// <param name="recursive">When <c>true</c>, values inherited from ancestors are included.</param>
		/// <param name="cancellationToken">Cancels the search; a recursive one walks the whole hierarchy above the concept.</param>
		/// <returns>The matching sign value statements.</returns>
		/// <exception cref="OperationCanceledException">The token was cancelled.</exception>
		public static async Task<List<SignValueStatement>> GetSignValuesAsync(IEnumerable<IStatement> statements, IConcept concept, System.Boolean recursive, CancellationToken cancellationToken = default)
		{
			var result = new List<SignValueStatement>();
			var signValues = await statements.OfType<SignValueStatement>().ToListAsync(cancellationToken);
			result.AddRange(signValues.Where(sv => sv.Concept == concept));

			if (recursive)
			{
				foreach (var parent in await statements.GetParentsOneLevelAsync<IConcept, IsStatement>(concept, cancellationToken: cancellationToken))
				{
					var parentSignValues = await GetSignValuesAsync(statements, parent, true, cancellationToken);
					foreach (var signValue in parentSignValues)
					{
						if (! await result.AnyAsync(sv => sv.Sign == signValue.Sign, cancellationToken))
						{
							result.Add(signValue);
						}
					}
				}
			}
			return result;
		}

		/// <summary>Blocking counterpart of <see cref="CheckHasSignAsync"/>.</summary>
		/// <param name="statements">Statements to search for the sign declaration.</param>
		/// <param name="cancellationToken">Cancels the search.</param>
		/// <returns><c>true</c> when a matching declaration exists.</returns>
		public System.Boolean CheckHasSign(IEnumerable<IStatement> statements, CancellationToken cancellationToken = default)
		{
			return TaskHelper.AwaitDetached(() => CheckHasSignAsync(statements, cancellationToken));
		}

		/// <summary>Blocking counterpart of <see cref="GetSignValueAsync"/>.</summary>
		/// <param name="statements">Statements to search.</param>
		/// <param name="concept">Concept whose sign value is wanted.</param>
		/// <param name="sign">Sign whose value is wanted.</param>
		/// <param name="cancellationToken">Cancels the search.</param>
		/// <returns>The matching statement, or <c>null</c> when the value is undefined.</returns>
		public static SignValueStatement GetSignValue(IEnumerable<IStatement> statements, IConcept concept, IConcept sign, CancellationToken cancellationToken = default)
		{
			return TaskHelper.AwaitDetached(() => GetSignValueAsync(statements, concept, sign, cancellationToken));
		}

		/// <summary>Blocking counterpart of <see cref="GetSignValuesAsync"/>.</summary>
		/// <param name="statements">Statements to search.</param>
		/// <param name="concept">Concept whose sign values are wanted.</param>
		/// <param name="recursive">When <c>true</c>, values inherited from ancestors are included.</param>
		/// <param name="cancellationToken">Cancels the search.</param>
		/// <returns>The matching sign value statements.</returns>
		public static List<SignValueStatement> GetSignValues(IEnumerable<IStatement> statements, IConcept concept, System.Boolean recursive, CancellationToken cancellationToken = default)
		{
			return TaskHelper.AwaitDetached(() => GetSignValuesAsync(statements, concept, recursive, cancellationToken));
		}
	}
}
