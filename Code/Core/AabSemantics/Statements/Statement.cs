using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AabSemantics.Localization;
using AabSemantics.Utils;

namespace AabSemantics.Statements
{
	/// <summary>Base <see cref="IStatement"/> implementation. Most statement types derive from
	/// <see cref="Statement{StatementT}"/> instead, which supplies the equality plumbing.</summary>
	public abstract class Statement : IStatement
	{
		#region Properties

		/// <summary>Localized display name of the statement type.</summary>
		public ILocalizedString Name
		{ get; }

		/// <summary>Identifier, unique within the semantic network.</summary>
		public String ID
		{ get; private set; }

		/// <summary>Context the statement belongs to; assigned when it is added to a network.</summary>
		public IContext Context
		{ get; set; }

		/// <summary>Localized tooltip text.</summary>
		public ILocalizedString Hint
		{ get; }

		#endregion

		/// <summary>Returns every concept the statement refers to.</summary>
		/// <returns>Concepts participating in the statement.</returns>
		public abstract IEnumerable<IConcept> GetChildConcepts();

		/// <summary>Formats the statement as <c>TypeName [ID]</c>.</summary>
		/// <returns>Diagnostic string.</returns>
		public sealed override String ToString()
		{
			return this.GetTypeWithId();
		}

		/// <summary>Initializes the statement, generating an identifier when none is given.</summary>
		/// <param name="id">Identifier; a GUID is generated when null or empty.</param>
		/// <param name="name">Localized display name.</param>
		/// <param name="hint">Localized tooltip text; empty when <c>null</c>.</param>
		/// <exception cref="ArgumentNullException"><paramref name="name"/> is <c>null</c>.</exception>
		protected Statement(String id, ILocalizedString name, ILocalizedString hint = null)
		{
			Update(id);

			Name = name.EnsureNotNull(nameof(name));

			Hint = hint ?? LocalizedString.Empty;
		}

		/// <summary>Reassigns the identifier.</summary>
		/// <param name="id">New identifier; a GUID is generated when null or empty.</param>
		public void Update(String id)
		{
			ID = id.EnsureIdIsSet();
		}

		/// <summary>Checks that the statement does not duplicate any of the given ones.</summary>
		/// <param name="statements">Statements to compare against.</param>
		/// <returns><c>true</c> if the statement carries new information.</returns>
		public abstract Task<Boolean> CheckUniqueAsync(IEnumerable<IStatement> statements);

#pragma warning disable 659
		/// <summary>Compares the statement with another object by value.</summary>
		/// <param name="obj">Object to compare with.</param>
		/// <returns><c>true</c> if they assert the same thing.</returns>
		public abstract override Boolean Equals(Object obj);

		/// <summary>
		/// Returns the reference-based hash code. Deliberately not aligned with
		/// <see cref="Equals(Object)"/>, so statements must not be used as dictionary keys.
		/// </summary>
		/// <returns>Reference-based hash code.</returns>
		public override Int32 GetHashCode()
		{
// ReSharper disable once BaseObjectGetHashCodeCallInGetHashCode
			return base.GetHashCode();
		}
#pragma warning restore 659
	}

	/// <summary>
	/// Base statement that derives uniqueness checking from value equality, leaving a subclass
	/// only <see cref="Equals(StatementT)"/> to implement.
	/// </summary>
	/// <typeparam name="StatementT">The deriving type itself.</typeparam>
	public abstract class Statement<StatementT> : Statement, IEquatable<StatementT>
		where StatementT : Statement<StatementT>
	{
		/// <summary>Initializes the statement.</summary>
		/// <param name="id">Identifier; a GUID is generated when null or empty.</param>
		/// <param name="name">Localized display name.</param>
		/// <param name="hint">Localized tooltip text; empty when <c>null</c>.</param>
		protected Statement(String id, LocalizedString name, LocalizedString hint = null)
			: base(id, name, hint)
		{ }

		/// <summary>
		/// Reports whether the statement is unique among the given ones. The statement is
		/// expected to be part of that collection, so a single equal item is itself; two mean
		/// a duplicate.
		/// </summary>
		/// <param name="statements">Statements to compare against.</param>
		/// <returns><c>false</c> once a second equal statement is found.</returns>
		public sealed override async Task<Boolean> CheckUniqueAsync(IEnumerable<IStatement> statements)
		{
			return await Task.Run(() =>
			{
				bool found = false;
				foreach (var _ in statements.OfType<StatementT>().Where(s => Equals(s)))
				{
					if (!found)
					{
						found = true;
					}
					else
					{
						return Task.FromResult(false);
					}
				}

				return Task.FromResult(true);
			}).ConfigureAwait(false);
		}

		/// <summary>Compares the statement with another of the same type by value.</summary>
		/// <param name="other">Statement to compare with; may be <c>null</c>.</param>
		/// <returns><c>true</c> if both assert the same thing.</returns>
		public abstract Boolean Equals(StatementT other);

#pragma warning disable 659
		/// <summary>Compares by value, treating objects of other types as unequal.</summary>
		/// <param name="obj">Object to compare with.</param>
		/// <returns><c>true</c> if both assert the same thing.</returns>
		public sealed override Boolean Equals(Object obj)
		{
			return Equals(obj as StatementT);
		}

		/// <summary>
		/// Returns the reference-based hash code, deliberately not aligned with
		/// <see cref="Equals(Object)"/>.
		/// </summary>
		/// <returns>Reference-based hash code.</returns>
		public override Int32 GetHashCode()
		{
			// ReSharper disable once BaseObjectGetHashCodeCallInGetHashCode
			return base.GetHashCode();
		}
#pragma warning restore 659
	}
}
