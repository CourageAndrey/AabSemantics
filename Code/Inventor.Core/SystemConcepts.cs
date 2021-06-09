using System;
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

		public static readonly ICollection<IConcept> LogicalValues = new HashSet<IConcept>
		{
			True,
			False,
		};

		#endregion

		#region Comparison signs

		public static readonly IConcept IsEqualTo = new Concept(
			new LocalizedStringConstant(lang => lang.SystemConceptNames.IsEqualTo),
			new LocalizedStringConstant(lang => lang.SystemConceptHints.IsEqualTo))
		{
			Attributes = { IsComparisonSignAttribute.Value },
		};

		public static readonly IConcept IsNotEqualTo = new Concept(
			new LocalizedStringConstant(lang => lang.SystemConceptNames.IsNotEqualTo),
			new LocalizedStringConstant(lang => lang.SystemConceptHints.IsNotEqualTo))
		{
			Attributes = { IsComparisonSignAttribute.Value },
		};

		public static readonly IConcept IsGreaterThanOrEqualTo = new Concept(
			new LocalizedStringConstant(lang => lang.SystemConceptNames.IsGreaterThanOrEqualTo),
			new LocalizedStringConstant(lang => lang.SystemConceptHints.IsGreaterThanOrEqualTo))
		{
			Attributes = { IsComparisonSignAttribute.Value },
		};

		public static readonly IConcept IsGreaterThan = new Concept(
			new LocalizedStringConstant(lang => lang.SystemConceptNames.IsGreaterThan),
			new LocalizedStringConstant(lang => lang.SystemConceptHints.IsGreaterThan))
		{
			Attributes = { IsComparisonSignAttribute.Value },
		};

		public static readonly IConcept IsLessThanOrEqualTo = new Concept(
			new LocalizedStringConstant(lang => lang.SystemConceptNames.IsLessThanOrEqualTo),
			new LocalizedStringConstant(lang => lang.SystemConceptHints.IsLessThanOrEqualTo))
		{
			Attributes = { IsComparisonSignAttribute.Value },
		};

		public static readonly IConcept IsLessThan = new Concept(
			new LocalizedStringConstant(lang => lang.SystemConceptNames.IsLessThan),
			new LocalizedStringConstant(lang => lang.SystemConceptHints.IsLessThan))
		{
			Attributes = { IsComparisonSignAttribute.Value },
		};

		public static readonly ICollection<IConcept> ComparisonSigns = new HashSet<IConcept>
		{
			IsEqualTo,
			IsNotEqualTo,
			IsGreaterThanOrEqualTo,
			IsGreaterThan,
			IsLessThanOrEqualTo,
			IsLessThan,
		};

		public static IConcept Revert(this IConcept comparisonSign)
		{
			if (!ComparisonSigns.Contains(comparisonSign)) throw new InvalidOperationException("Only comparison signs can be reverted using this method.");

			if (comparisonSign == IsGreaterThanOrEqualTo)
			{
				return IsLessThanOrEqualTo;
			}
			else if (comparisonSign == IsGreaterThan)
			{
				return IsLessThan;
			}
			else if (comparisonSign == IsLessThanOrEqualTo)
			{
				return IsGreaterThanOrEqualTo;
			}
			else if (comparisonSign == IsLessThan)
			{
				return IsGreaterThan;
			}
			else
			{
				return comparisonSign;
			}
		}

		#endregion

		#region Sequence signs

		public static readonly IConcept StartsAfterOtherStarted = new Concept(
			new LocalizedStringConstant(lang => lang.SystemConceptNames.StartsAfterOtherStarted),
			new LocalizedStringConstant(lang => lang.SystemConceptHints.StartsAfterOtherStarted))
		{
			Attributes = { IsSequenceSignAttribute.Value },
		};

		public static readonly IConcept StartsBeforeOtherStarted = new Concept(
			new LocalizedStringConstant(lang => lang.SystemConceptNames.StartsBeforeOtherStarted),
			new LocalizedStringConstant(lang => lang.SystemConceptHints.StartsBeforeOtherStarted))
		{
			Attributes = { IsSequenceSignAttribute.Value },
		};

		public static readonly IConcept FinishesAfterOtherStarted = new Concept(
			new LocalizedStringConstant(lang => lang.SystemConceptNames.FinishesAfterOtherStarted),
			new LocalizedStringConstant(lang => lang.SystemConceptHints.FinishesAfterOtherStarted))
		{
			Attributes = { IsSequenceSignAttribute.Value },
		};

		public static readonly IConcept FinishesBeforeOtherStarted = new Concept(
			new LocalizedStringConstant(lang => lang.SystemConceptNames.FinishesBeforeOtherStarted),
			new LocalizedStringConstant(lang => lang.SystemConceptHints.FinishesBeforeOtherStarted))
		{
			Attributes = { IsSequenceSignAttribute.Value },
		};

		public static readonly IConcept StartsAfterOtherFinished = new Concept(
			new LocalizedStringConstant(lang => lang.SystemConceptNames.StartsAfterOtherFinished),
			new LocalizedStringConstant(lang => lang.SystemConceptHints.StartsAfterOtherFinished))
		{
			Attributes = { IsSequenceSignAttribute.Value },
		};

		public static readonly IConcept StartsBeforeOtherFinished = new Concept(
			new LocalizedStringConstant(lang => lang.SystemConceptNames.StartsBeforeOtherFinished),
			new LocalizedStringConstant(lang => lang.SystemConceptHints.StartsBeforeOtherFinished))
		{
			Attributes = { IsSequenceSignAttribute.Value },
		};

		public static readonly IConcept FinishesAfterOtherFinished = new Concept(
			new LocalizedStringConstant(lang => lang.SystemConceptNames.FinishesAfterOtherFinished),
			new LocalizedStringConstant(lang => lang.SystemConceptHints.FinishesAfterOtherFinished))
		{
			Attributes = { IsSequenceSignAttribute.Value },
		};

		public static readonly IConcept FinishesBeforeOtherFinished = new Concept(
			new LocalizedStringConstant(lang => lang.SystemConceptNames.FinishesBeforeOtherFinished),
			new LocalizedStringConstant(lang => lang.SystemConceptHints.FinishesBeforeOtherFinished))
		{
			Attributes = { IsSequenceSignAttribute.Value },
		};

		public static readonly IConcept Causes = new Concept(
			new LocalizedStringConstant(lang => lang.SystemConceptNames.Causes),
			new LocalizedStringConstant(lang => lang.SystemConceptHints.Causes))
		{
			Attributes = { IsSequenceSignAttribute.Value },
		};

		public static readonly IConcept SimultaneousWith = new Concept(
			new LocalizedStringConstant(lang => lang.SystemConceptNames.SimultaneousWith),
			new LocalizedStringConstant(lang => lang.SystemConceptHints.SimultaneousWith))
		{
			Attributes = { IsSequenceSignAttribute.Value },
		};

		public static readonly ICollection<IConcept> SequenceSigns = new HashSet<IConcept>
		{
			StartsAfterOtherStarted,
			StartsBeforeOtherStarted,
			FinishesAfterOtherStarted,
			FinishesBeforeOtherStarted,
			StartsAfterOtherFinished,
			StartsBeforeOtherFinished,
			FinishesAfterOtherFinished,
			FinishesBeforeOtherFinished,
			Causes,
			SimultaneousWith,
		};

		#endregion

		#endregion

		public static IEnumerable<IConcept> GetAll()
		{
			foreach (var concept in LogicalValues)
			{
				yield return concept;
			}

			foreach (var concept in ComparisonSigns)
			{
				yield return concept;
			}

			foreach (var concept in SequenceSigns)
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
