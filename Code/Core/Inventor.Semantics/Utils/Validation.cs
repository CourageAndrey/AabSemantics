using System;

namespace Inventor.Semantics.Utils
{
	public static class Validation
	{
		public static T EnsureNotNull<T>(this T parameter, String parameterName)
		{
			if (parameter == null)
			{
				throw new ArgumentNullException(parameterName);
			}
			else
			{
				return parameter;
			}
		}

		public static ConceptT EnsureHasAttribute<ConceptT, AttributeT>(this ConceptT concept, String parameterName)
			where ConceptT : IConcept
			where AttributeT : IAttribute
		{
			if (!concept.HasAttribute<AttributeT>())
			{
				String capitalized = parameterName.Remove(1).ToUpperInvariant() + parameterName.Substring(1);
				String attributeName = typeof(AttributeT).Name.Replace("Attribute", String.Empty);

				throw new ArgumentException($"{capitalized} concept has to be marked as {attributeName} Attribute.", nameof(parameterName));
			}
			else
			{
				return concept;
			}
		}
	}
}
