using System;
using System.Collections.Generic;

using Inventor.Core.Attributes;
using Inventor.Core.Localization;

namespace Inventor.Core.Concepts
{
	public static class LogicalValues
	{
		#region Properties

		public static readonly IConcept True = new SystemConcept($"{{{nameof(LogicalValues)}.{nameof(True)}}}",
			new LocalizedStringConstant(lang => lang.Concepts.SystemConceptNames.True), new LocalizedStringConstant(lang => lang.Concepts.SystemConceptHints.True));

		public static readonly IConcept False = new SystemConcept($"{{{nameof(LogicalValues)}.{nameof(False)}}}",
			new LocalizedStringConstant(lang => lang.Concepts.SystemConceptNames.False), new LocalizedStringConstant(lang => lang.Concepts.SystemConceptHints.False));

		public static readonly ICollection<IConcept> All = new HashSet<IConcept>
		{
			True,
			False,
		};

		#endregion

		private static void ensureSuits(this IConcept value)
		{
			if (!All.Contains(value))
			{
				throw new InvalidOperationException("This method can work only with logical values.");
			}
		}

		public static IConcept Invert(this IConcept value)
		{
			ensureSuits(value);

			if (value == True)
			{
				return False;
			}
			else if (value == False)
			{
				return True;
			}
			else
			{
				return value;
			}
		}

		public static Boolean ToBoolean(this IConcept value)
		{
			ensureSuits(value);

			if (value == True)
			{
				return true;
			}
			else if (value == False)
			{
				return false;
			}
			else
			{
				throw new NotSupportedException();
			}
		}

		public static IConcept ToLogicalValue(this Boolean value)
		{
			return value
				? True
				: False;
		}

		static LogicalValues()
		{
			foreach (var concept in All)
			{
				concept.WithAttribute(IsBooleanAttribute.Value);
			}
		}
	}
}