using System.Collections.Generic;

using Inventor.Semantics.Modules.Mathematics.Concepts;
using Inventor.Semantics.Modules.Boolean.Concepts;
using Inventor.Semantics.Modules.Processes.Concepts;

namespace Inventor.Semantics.Test
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
