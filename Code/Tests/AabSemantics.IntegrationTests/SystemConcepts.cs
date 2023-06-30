using System.Collections.Generic;

using AabSemantics.Modules.Boolean.Concepts;
using AabSemantics.Modules.Mathematics.Concepts;
using AabSemantics.Modules.Processes.Concepts;

namespace AabSemantics.IntegrationTests
{
	public static class SystemConcepts
	{
		public static IEnumerable<IConcept> GetAll()
		{
			foreach (var concept in LogicalValues.All)
			{
				yield return concept;
			}

			foreach (var concept in ComparisonSigns.All)
			{
				yield return concept;
			}

			foreach (var concept in SequenceSigns.All)
			{
				yield return concept;
			}
		}
	}
}
