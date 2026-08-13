using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AabSemantics.Serialization
{
	/// <summary>
	/// Turns concept identifiers back into concepts while deserializing. Module-provided system
	/// concepts are looked up in a process-wide registry and take precedence over the concepts
	/// of the network being loaded.
	/// </summary>
	public class ConceptIdResolver
	{
		/// <summary>
		/// System concepts registered by modules, shared across the process and searched before
		/// the network's own concepts.
		/// </summary>
		public static readonly IDictionary<String, IConcept> SystemConceptsById = new Dictionary<String, IConcept>();

		private readonly IDictionary<String, IConcept> _conceptsById = new Dictionary<String, IConcept>();

		/// <summary>
		/// Registers every concept exposed as a public static <see cref="IConcept"/> field of a
		/// type, which is how a module publishes its system concepts.
		/// </summary>
		/// <param name="type">Type whose static concept fields are scanned.</param>
		public static void RegisterEnumType(Type type)
		{
			foreach (var field in type.GetFields(BindingFlags.GetField | BindingFlags.Static | BindingFlags.Public).Where(f => f.FieldType == typeof(IConcept)))
			{
				IConcept concept = (IConcept) field.GetValue(null);
				SystemConceptsById[concept.ID] = concept;
			}
		}

		/// <summary>Creates a resolver over the concepts of the network being loaded.</summary>
		/// <param name="concepts">Concepts keyed by identifier; copied into the resolver.</param>
		public ConceptIdResolver(IDictionary<String, IConcept> concepts)
		{
			foreach (var concept in concepts)
			{
				_conceptsById[concept.Key] = concept.Value;
			}
		}

		/// <summary>Resolves an identifier, checking the system concepts first.</summary>
		/// <param name="id">Concept identifier.</param>
		/// <returns>The matching concept.</returns>
		/// <exception cref="KeyNotFoundException">Neither a system concept nor a network concept has that identifier.</exception>
		public IConcept GetConceptById(String id)
		{
			IConcept systemConcept;
			return SystemConceptsById.TryGetValue(id, out systemConcept)
				? systemConcept
				: _conceptsById[id];
		}
	}
}
