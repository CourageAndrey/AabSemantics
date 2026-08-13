using System;
using System.Collections.Generic;

using AabSemantics.Modules.Set.Localization;
using AabSemantics.Statements;
using AabSemantics.Utils;

namespace AabSemantics.Modules.Set.Statements
{
	/// <summary>States that one concept is part of another. Traversable as a hierarchy, with the whole as parent.</summary>
	public class HasPartStatement : Statement<HasPartStatement>, IParentChild<IConcept>
	{
		#region Properties

		/// <summary>The containing concept.</summary>
		public IConcept Whole
		{ get; private set; }

		/// <summary>The contained concept.</summary>
		public IConcept Part
		{ get; private set; }

		/// <summary>Same as the containing side, under the generic hierarchy naming.</summary>
		public IConcept Parent
		{ get { return Whole; } }

		/// <summary>Same as the contained side, under the generic hierarchy naming.</summary>
		public IConcept Child
		{ get { return Part; } }

		#endregion

		/// <summary>Creates the statement.</summary>
		/// <param name="id">Identifier; a GUID is generated when null or empty.</param>
		/// <param name="whole">The containing concept.</param>
		/// <param name="part">The contained concept.</param>
		/// <exception cref="System.ArgumentNullException">A required concept is <c>null</c>.</exception>
		public HasPartStatement(String id, IConcept whole, IConcept part)
			: base(
				id,
				new Func<ILanguage, String>(language => language.GetStatementsExtension<ILanguageSetModule, ILanguageStatements>().Names.Composition),
				new Func<ILanguage, String>(language => language.GetStatementsExtension<ILanguageSetModule, ILanguageStatements>().Hints.Composition))
		{
			Update(id, whole, part);
		}

		/// <summary>Reassigns the identifier and the related concepts.</summary>
		/// <param name="id">New identifier; a GUID is generated when null or empty.</param>
		/// <param name="whole">The containing concept.</param>
		/// <param name="part">The contained concept.</param>
		/// <exception cref="System.ArgumentNullException">A required concept is <c>null</c>.</exception>
		public void Update(String id, IConcept whole, IConcept part)
		{
			Update(id);
			Whole = whole.EnsureNotNull(nameof(whole));
			Part = part.EnsureNotNull(nameof(part));
		}

		/// <summary>Returns the concepts the statement relates.</summary>
		/// <returns>The participating concepts.</returns>
		public override IEnumerable<IConcept> GetChildConcepts()
		{
			yield return Whole;
			yield return Part;
		}

		#region Consistency checking

		/// <summary>Compares the related concepts by reference.</summary>
		/// <param name="other">Statement to compare with; may be <c>null</c>.</param>
		/// <returns><c>true</c> if both relate the same concepts.</returns>
		public override System.Boolean Equals(HasPartStatement other)
		{
			if (ReferenceEquals(this, other)) return true;
			if (other != null)
			{
				return	other.Whole == Whole &&
						other.Part == Part;
			}
			else return false;
		}

		#endregion
	}
}
