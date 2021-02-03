using System.Collections.Generic;

using Inventor.Core.Attributes;
using Inventor.Core.Base;
using Inventor.Core.Localization;

namespace Inventor.Core
{
	public static class SystemConcepts
	{
		#region Properties

		#region Logical values

		public static readonly IConcept True = new Concept(
			new LocalizedStringConstant(lang => lang.SystemConceptNames.True),
			new LocalizedStringConstant(lang => lang.SystemConceptHints.True));

		public static readonly IConcept False = new Concept(
			new LocalizedStringConstant(lang => lang.SystemConceptNames.False),
			new LocalizedStringConstant(lang => lang.SystemConceptHints.False));

		public static readonly ICollection<IConcept> LogicalValues = new HashSet<IConcept>
		{
			True,
			False,
		};

		#endregion

		#endregion

		public static IEnumerable<IConcept> GetAll()
		{
			foreach (var concept in LogicalValues)
			{
				yield return concept;
			}
		}

		static SystemConcepts()
		{
			foreach (var concept in GetAll())
			{
				concept.Attributes.Add(IsValueAttribute.Value);
			}
		}
	}
}
