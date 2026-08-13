using System;
using System.Collections.Generic;

using AabSemantics.Modules.Set.Localization;
using AabSemantics.Statements;
using AabSemantics.Utils;

namespace AabSemantics.Modules.Set.Statements
{
	/// <summary>States that a concept belongs to a subject area. Traversable as a hierarchy, with the area as parent.</summary>
	public class GroupStatement : Statement<GroupStatement>, IParentChild<IConcept>
	{
		#region Properties

		/// <summary>The subject area concept.</summary>
		public IConcept Area
		{ get; private set; }

		/// <summary>The concept in question.</summary>
		public IConcept Concept
		{ get; private set; }

		/// <summary>Same as the containing side, under the generic hierarchy naming.</summary>
		public IConcept Parent
		{ get { return Area; } }

		/// <summary>Same as the contained side, under the generic hierarchy naming.</summary>
		public IConcept Child
		{ get { return Concept; } }

		#endregion

		/// <summary>Creates the statement.</summary>
		/// <param name="id">Identifier; a GUID is generated when null or empty.</param>
		/// <param name="area">The subject area concept.</param>
		/// <param name="concept">The concept in question.</param>
		/// <exception cref="System.ArgumentNullException">A required concept is <c>null</c>.</exception>
		public GroupStatement(String id, IConcept area, IConcept concept)
			: base(
				id,
				new Func<ILanguage, String>(language => language.GetStatementsExtension<ILanguageSetModule, ILanguageStatements>().Names.SubjectArea),
				new Func<ILanguage, String>(language => language.GetStatementsExtension<ILanguageSetModule, ILanguageStatements>().Hints.SubjectArea))
		{
			Update(id, area, concept);
		}

		/// <summary>Reassigns the identifier and the related concepts.</summary>
		/// <param name="id">New identifier; a GUID is generated when null or empty.</param>
		/// <param name="area">The subject area concept.</param>
		/// <param name="concept">The concept in question.</param>
		/// <exception cref="System.ArgumentNullException">A required concept is <c>null</c>.</exception>
		public void Update(String id, IConcept area, IConcept concept)
		{
			Update(id);
			Area = area.EnsureNotNull(nameof(area));
			Concept = concept.EnsureNotNull(nameof(concept));
		}

		/// <summary>Returns the concepts the statement relates.</summary>
		/// <returns>The participating concepts.</returns>
		public override IEnumerable<IConcept> GetChildConcepts()
		{
			yield return Area;
			yield return Concept;
		}

		#region Consistency checking

		/// <summary>Compares the related concepts by reference.</summary>
		/// <param name="other">Statement to compare with; may be <c>null</c>.</param>
		/// <returns><c>true</c> if both relate the same concepts.</returns>
		public override System.Boolean Equals(GroupStatement other)
		{
			if (ReferenceEquals(this, other)) return true;
			if (other != null)
			{
				return	other.Area == Area &&
						other.Concept == Concept;
			}
			else return false;
		}

		#endregion
	}
}
