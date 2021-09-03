using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Inventor.Core.Concepts;

namespace Inventor.Core.Xml
{
	public class ConceptIdResolver
	{
		private static readonly IDictionary<String, IConcept> _systemConceptsById = new Dictionary<String, IConcept>();
		private readonly IDictionary<String, IConcept> _conceptsById = new Dictionary<String, IConcept>();

		static ConceptIdResolver()
		{
			foreach (var type in new[] { typeof(LogicalValues), typeof(ComparisonSigns), typeof(SequenceSigns) })
			{
				RegisterEnumType(type);
			}
		}

		public static void RegisterEnumType(Type type)
		{
			foreach (var field in type.GetFields(BindingFlags.GetField | BindingFlags.Static | BindingFlags.Public).Where(f => f.FieldType == typeof(IConcept)))
			{
				IConcept concept = (IConcept) field.GetValue(null);
				_systemConceptsById[concept.ID] = concept;
			}
		}

		public ConceptIdResolver(IDictionary<Concept, IConcept> concepts)
		{
			foreach (var concept in concepts)
			{
				_conceptsById[concept.Key.ID] = concept.Value;
			}
		}

		public IConcept GetConceptById(String id)
		{
			IConcept systemConcept;
			return _systemConceptsById.TryGetValue(id, out systemConcept)
				? systemConcept
				: _conceptsById[id];
		}
	}
}
