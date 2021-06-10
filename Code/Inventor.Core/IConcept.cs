using System;
using System.Collections.Generic;
using System.Linq;

namespace Inventor.Core
{
	public interface IConcept : INamed
	{
		ILocalizedString Hint
		{ get; }

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
	}
}
