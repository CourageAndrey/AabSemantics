using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Inventor.Semantics.Xml
{
	public class ConceptIdResolver
	{
		public static readonly IDictionary<String, IConcept> SystemConceptsById = new Dictionary<String, IConcept>();

		private readonly IDictionary<String, IConcept> _conceptsById = new Dictionary<String, IConcept>();

		public static void RegisterEnumType(Type type)
		{
			foreach (var field in type.GetFields(BindingFlags.GetField | BindingFlags.Static | BindingFlags.Public).Where(f => f.FieldType == typeof(IConcept)))
			{
				IConcept concept = (IConcept) field.GetValue(null);
				SystemConceptsById[concept.ID] = concept;
			}
		}

		public ConceptIdResolver(IDictionary<String, IConcept> concepts)
		{
			foreach (var concept in concepts)
			{
				_conceptsById[concept.Key] = concept.Value;
			}
		}

		public IConcept GetConceptById(String id)
		{
			IConcept systemConcept;
			return SystemConceptsById.TryGetValue(id, out systemConcept)
				? systemConcept
				: _conceptsById[id];
		}
	}
}
