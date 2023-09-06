using System;
using System.Collections.Generic;

using AabSemantics.Localization;

namespace AabSemantics.Concepts
{
	public class Concept : IConcept
	{
		#region Properties

		public ILocalizedString Name
		{ get; }

		public String ID
		{ get; private set; }

		public ILocalizedString Hint
		{ get; }

		public ICollection<IAttribute> Attributes
		{ get; }

		#endregion

		#region Constructors

		public Concept(String id = null, ILocalizedString name = null, ILocalizedString hint = null)
		{
			Name = name ?? new LocalizedStringVariable();
			ID = id.EnsureIdIsSet();
			Hint = hint ?? new LocalizedStringVariable();
			Attributes = new HashSet<IAttribute>();
		}

		#endregion

		public virtual void UpdateIdIfAllowed(String id)
		{
			ID = id.EnsureIdIsSet();
		}

		public override String ToString()
		{
			return this.GetTypeWithId();
		}
	}

	public static class ConceptCreationHelper
	{
		public static IConcept CreateEmptyConcept()
		{
			return new Concept(String.Empty, new LocalizedStringConstant(language => String.Empty));
		}

		public static IConcept CreateConceptByObject(this Object @object)
		{
			String text = @object.ToString();
			return new Concept(text, new LocalizedStringConstant(language => text));
		}

		public static IConcept CreateConceptByName(this String name, String id = null)
		{
			if (String.IsNullOrEmpty(id))
			{
				id = name;
			}
			return new Concept(id, new LocalizedStringConstant(language => name));
		}

		public static IConcept CreateConceptById(this String id, String name = null)
		{
			return new Concept(id, new LocalizedStringConstant(language => name));
		}
	}
}
