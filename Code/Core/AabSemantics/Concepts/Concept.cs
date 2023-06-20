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
		public static IConcept CreateConcept()
		{
			return String.Empty.CreateConcept();
		}

		public static IConcept CreateConcept(this Object @object)
		{
			return @object.ToString().CreateConcept();
		}

		public static IConcept CreateConcept(this String name)
		{
			return name.CreateConcept(name);
		}

		public static IConcept CreateConcept(this String id, String name)
		{
			return new Concept(id, new LocalizedStringConstant(language => name));
		}
	}
}
