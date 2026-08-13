using System;

namespace AabSemantics.Utils
{
	/// <summary>Argument guards that return the checked value, so they can be used inline in assignments.</summary>
	public static class Validation
	{
		/// <summary>Throws when the value is <c>null</c>, otherwise returns it.</summary>
		/// <typeparam name="T">Value type.</typeparam>
		/// <param name="parameter">Value to check.</param>
		/// <param name="parameterName">Name reported in the exception.</param>
		/// <returns>The value itself.</returns>
		/// <exception cref="ArgumentNullException">The value is <c>null</c>.</exception>
		public static T EnsureNotNull<T>(this T parameter, String parameterName)
			where T : class
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

		/// <summary>Throws unless the concept carries the given attribute, otherwise returns it.</summary>
		/// <typeparam name="ConceptT">Concept type.</typeparam>
		/// <typeparam name="AttributeT">Attribute the concept must carry.</typeparam>
		/// <param name="concept">Concept to check.</param>
		/// <param name="parameterName">Name reported in the exception message, capitalized there.</param>
		/// <returns>The concept itself.</returns>
		/// <exception cref="ArgumentException">The concept lacks the attribute.</exception>
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

		/// <summary>Throws unless the type is instantiable and satisfies a contract, otherwise returns it.</summary>
		/// <typeparam name="ContractT">Contract the type must satisfy.</typeparam>
		/// <param name="type">Type to check.</param>
		/// <param name="parameterName">Name reported in the exception.</param>
		/// <returns>The type itself.</returns>
		/// <exception cref="ArgumentException">The type is abstract or does not satisfy the contract.</exception>
		public static Type EnsureContract<ContractT>(this Type type, String parameterName)
		{
			return type.EnsureContract(typeof(ContractT), parameterName);
		}

		/// <summary>Throws unless the type is instantiable and satisfies a contract, otherwise returns it.</summary>
		/// <param name="type">Type to check.</param>
		/// <param name="contractType">Contract the type must satisfy.</param>
		/// <param name="parameterName">Name reported in the exception.</param>
		/// <returns>The type itself.</returns>
		/// <exception cref="ArgumentException">The type is abstract or does not satisfy the contract.</exception>
		public static Type EnsureContract(this Type type, Type contractType, String parameterName)
		{
			if (type.IsAbstract || !contractType.IsAssignableFrom(type))
			{
				throw new ArgumentException($"Kind must be non-abstract and implement {contractType}.", parameterName);
			}
			else
			{
				return type;
			}
		}
	}
}
