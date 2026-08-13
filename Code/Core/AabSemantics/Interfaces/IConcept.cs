using System;
using System.Collections.Generic;
using System.Linq;

namespace AabSemantics
{
	/// <summary>
	/// A node of the semantic network: an entity that statements are made about.
	/// Its <see cref="Attributes"/> classify it and drive which statements and questions apply to it.
	/// </summary>
	public interface IConcept : IKnowledge
	{
		/// <summary>
		/// Attributes attached to the concept. The collection is mutable; use the
		/// <see cref="ConceptAttributesExtension"/> helpers for fluent modification.
		/// </summary>
		ICollection<IAttribute> Attributes
		{ get; }
	}

	/// <summary>
	/// Fluent helpers for inspecting and modifying <see cref="IConcept.Attributes"/>.
	/// </summary>
	public static class ConceptAttributesExtension
	{
		/// <summary>
		/// Determines whether the concept carries an attribute of the given type.
		/// </summary>
		/// <typeparam name="AttributeT">Attribute type to look for.</typeparam>
		/// <param name="concept">Concept to inspect.</param>
		/// <returns><c>true</c> if at least one attribute of that type is present.</returns>
		public static Boolean HasAttribute<AttributeT>(this IConcept concept)
			where AttributeT : IAttribute
		{
			return concept.Attributes.OfType<AttributeT>().Any();
		}

		/// <summary>
		/// Adds a single attribute to the concept.
		/// </summary>
		/// <param name="concept">Concept to modify in place.</param>
		/// <param name="attribute">Attribute to add.</param>
		/// <returns>The same concept, to allow call chaining.</returns>
		public static IConcept WithAttribute(this IConcept concept, IAttribute attribute)
		{
			concept.Attributes.Add(attribute);
			return concept;
		}

		/// <summary>
		/// Adds several attributes to the concept.
		/// </summary>
		/// <param name="concept">Concept to modify in place.</param>
		/// <param name="attributes">Attributes to add.</param>
		/// <returns>The same concept, to allow call chaining.</returns>
		public static IConcept WithAttributes(this IConcept concept, IEnumerable<IAttribute> attributes)
		{
			foreach (var attribute in attributes)
			{
				concept.Attributes.Add(attribute);
			}
			return concept;
		}

		/// <summary>
		/// Removes every attribute from the concept.
		/// </summary>
		/// <param name="concept">Concept to modify in place.</param>
		/// <returns>The same concept, to allow call chaining.</returns>
		public static IConcept WithoutAttributes(this IConcept concept)
		{
			concept.Attributes.Clear();
			return concept;
		}
	}
}
