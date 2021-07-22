using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Inventor.Core.Xml
{
	public abstract class ConceptIdResolver
	{
		protected static readonly IDictionary<String, IConcept> SystemConceptsById = new Dictionary<String, IConcept>();
		protected static readonly IDictionary<IConcept, String> SystemConceptIds = new Dictionary<IConcept, String>();

		static ConceptIdResolver()
		{
			foreach (var type in new[] { typeof(LogicalValues), typeof(ComparisonSigns), typeof(SequenceSigns) })
			{
				foreach (var field in type.GetFields(BindingFlags.GetField | BindingFlags.Static | BindingFlags.Public).Where(f => f.FieldType == typeof(IConcept)))
				{
					String id = field.Name;
					IConcept concept = (IConcept)field.GetValue(null);
					SystemConceptsById[id] = concept;
					SystemConceptIds[concept] = id;
				}
			}
		}
	}

	public class LoadIdResolver : ConceptIdResolver
	{
		private readonly IDictionary<IConcept, Int32> _conceptIds;

		public LoadIdResolver(IDictionary<IConcept, Int32> concepts)
		{
			_conceptIds = concepts;
		}

		public String GetConceptId(IConcept concept)
		{
			String systemId;
			return SystemConceptIds.TryGetValue(concept, out systemId)
				? systemId
				: _conceptIds[concept].ToString();
		}
	}

	public class SaveIdResolver : ConceptIdResolver
	{
		private readonly IDictionary<Int32, IConcept> _conceptsById = new Dictionary<Int32, IConcept>();

		public SaveIdResolver(IDictionary<Concept, IConcept> concepts)
		{
			foreach (var concept in concepts)
			{
				_conceptsById[concept.Key.ID] = concept.Value;
			}
		}

		public IConcept GetConceptById(String id)
		{
			IConcept concept;
			return SystemConceptsById.TryGetValue(id, out concept)
				? concept
				: _conceptsById[Int32.Parse(id)];
		}
	}
}
