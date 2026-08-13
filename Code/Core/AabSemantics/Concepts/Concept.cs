using System;
using System.Collections.Generic;

using AabSemantics.Localization;

namespace AabSemantics.Concepts
{
	/// <summary>Default <see cref="IConcept"/> implementation.</summary>
	public class Concept : IConcept
	{
		#region Properties

		/// <summary>Localized display name.</summary>
		public ILocalizedString Name
		{ get; }

		/// <summary>Identifier, unique within the semantic network.</summary>
		public String ID
		{ get; private set; }

		/// <summary>Localized tooltip text.</summary>
		public ILocalizedString Hint
		{ get; }

		/// <summary>Attributes classifying the concept; duplicates are ignored.</summary>
		public ICollection<IAttribute> Attributes
		{ get; }

		#endregion

		#region Constructors

		/// <summary>Creates a concept, generating an identifier and empty strings for anything omitted.</summary>
		/// <param name="id">Identifier; a GUID is generated when null or empty.</param>
		/// <param name="name">Localized display name; an empty editable string when <c>null</c>.</param>
		/// <param name="hint">Localized tooltip text; an empty editable string when <c>null</c>.</param>
		public Concept(String id = null, ILocalizedString name = null, ILocalizedString hint = null)
		{
			Name = name ?? new LocalizedStringVariable();
			ID = id.EnsureIdIsSet();
			Hint = hint ?? new LocalizedStringVariable();
			Attributes = new HashSet<IAttribute>();
		}

		#endregion

		/// <summary>Reassigns the identifier. Derived types may refuse.</summary>
		/// <param name="id">New identifier; a GUID is generated when null or empty.</param>
		public virtual void UpdateIdIfAllowed(String id)
		{
			ID = id.EnsureIdIsSet();
		}

		/// <summary>Formats the concept as <c>TypeName [ID]</c>.</summary>
		/// <returns>Diagnostic string.</returns>
		public override String ToString()
		{
			return this.GetTypeWithId();
		}
	}

	/// <summary>Shorthands for creating concepts whose name is the same in every language.</summary>
	public static class ConceptCreationHelper
	{
		/// <summary>Creates a concept with an empty identifier and an empty name.</summary>
		/// <returns>The new concept.</returns>
		public static IConcept CreateEmptyConcept()
		{
			return new Concept(String.Empty, new LocalizedStringConstant(language => String.Empty));
		}

		/// <summary>Creates a concept named and identified by an object's string representation.</summary>
		/// <param name="object">Object to derive the identifier and name from.</param>
		/// <returns>The new concept.</returns>
		public static IConcept CreateConceptByObject(this Object @object)
		{
			String text = @object.ToString();
			return new Concept(text, new LocalizedStringConstant(language => text));
		}

		/// <summary>Creates a concept from its name.</summary>
		/// <param name="name">Display name, used in every language.</param>
		/// <param name="id">Identifier; the name is reused when null or empty.</param>
		/// <returns>The new concept.</returns>
		public static IConcept CreateConceptByName(this String name, String id = null)
		{
			if (String.IsNullOrEmpty(id))
			{
				id = name;
			}
			return new Concept(id, new LocalizedStringConstant(language => name));
		}

		/// <summary>Creates a concept from its identifier.</summary>
		/// <param name="id">Identifier.</param>
		/// <param name="name">Display name, used in every language; may be <c>null</c>.</param>
		/// <returns>The new concept.</returns>
		public static IConcept CreateConceptById(this String id, String name = null)
		{
			return new Concept(id, new LocalizedStringConstant(language => name));
		}
	}
}
