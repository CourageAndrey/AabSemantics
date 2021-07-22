using System.Collections.Generic;

using Inventor.Core.Attributes;
using Inventor.Core.Base;
using Inventor.Core.Localization;

namespace Inventor.Core
{
	public static class LogicalValues
	{
		#region Properties

		public static readonly IConcept True = new Concept(
			new LocalizedStringConstant(lang => lang.SystemConceptNames.True),
			new LocalizedStringConstant(lang => lang.SystemConceptHints.True))
		{
			Attributes = { IsBooleanAttribute.Value },
		};

		public static readonly IConcept False = new Concept(
			new LocalizedStringConstant(lang => lang.SystemConceptNames.False),
			new LocalizedStringConstant(lang => lang.SystemConceptHints.False))
		{
			Attributes = { IsBooleanAttribute.Value },
		};

		public static readonly ICollection<IConcept> All = new HashSet<IConcept>
		{
			True,
			False,
		};

		#endregion

		static LogicalValues()
		{
			foreach (var concept in All)
			{
				concept.Attributes.Add(IsValueAttribute.Value);
			}
		}
	}
}