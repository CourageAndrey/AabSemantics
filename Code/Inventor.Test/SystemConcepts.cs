using System.Collections.Generic;

using Inventor.Core;
using Inventor.Core.Concepts;
using Inventor.Mathematics.Concepts;
using Inventor.Processes.Concepts;

namespace Inventor.Test
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
