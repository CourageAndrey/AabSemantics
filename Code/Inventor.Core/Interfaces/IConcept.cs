using System;
using System.Collections.Generic;
using System.Linq;

namespace Inventor.Core
{
	public interface IConcept : IKnowledge
	{
		ICollection<IAttribute> Attributes
		{ get; }
	}

	public static class ConceptAttributesExtension
	{
		public static Boolean HasAttribute<AttributeT>(this IConcept concept)
			where AttributeT : IAttribute
		{
			return concept.Attributes.OfType<AttributeT>().Any();
		}

		public static IConcept WithAttribute(this IConcept concept, IAttribute attribute)
		{
			concept.Attributes.Add(attribute);
			return concept;
		}

		public static IConcept WithAttributes(this IConcept concept, IEnumerable<IAttribute> attributes)
		{
			foreach (var attribute in attributes)
			{
				concept.Attributes.Add(attribute);
			}
			return concept;
		}

		public static IConcept WithoutAttributes(this IConcept concept)
		{
			concept.Attributes.Clear();
			return concept;
		}
	}
}
