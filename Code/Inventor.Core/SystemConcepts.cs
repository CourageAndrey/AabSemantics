using System;
using System.Collections.Generic;
using System.Linq;
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

	public static class ComparisonSigns
	{
		#region Properties

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

		public static readonly ICollection<IConcept> All = new HashSet<IConcept>
		{
			IsEqualTo,
			IsNotEqualTo,
			IsGreaterThanOrEqualTo,
			IsGreaterThan,
			IsLessThanOrEqualTo,
			IsLessThan,
		};

		public static readonly ICollection<Tuple<IConcept, IConcept>> Contradictions = new List<Tuple<IConcept, IConcept>>
		{
			new Tuple<IConcept, IConcept>(IsEqualTo, IsNotEqualTo),
			new Tuple<IConcept, IConcept>(IsEqualTo, IsGreaterThan),
			new Tuple<IConcept, IConcept>(IsEqualTo, IsLessThan),
			new Tuple<IConcept, IConcept>(IsGreaterThan, IsLessThan),
			new Tuple<IConcept, IConcept>(IsGreaterThan, IsLessThanOrEqualTo),
			new Tuple<IConcept, IConcept>(IsLessThan, IsGreaterThanOrEqualTo),
		};

		#endregion

		public static Boolean Contradicts(this IConcept sign1, IConcept sign2)
		{
			ensureSuits(sign1);
			ensureSuits(sign2);

			return Contradictions.Any(tuple =>
				tuple.Item1 == sign2 && tuple.Item2 == sign1 ||
				tuple.Item1 == sign1 && tuple.Item2 == sign2);
		}

		private static void ensureSuits(this IConcept sign)
		{
			if (!All.Contains(sign))
			{
				throw new InvalidOperationException("This method can work only with comparison signs.");
			}
		}

		public static IConcept Revert(this IConcept sign)
		{
			ensureSuits(sign);

			if (sign == IsGreaterThanOrEqualTo)
			{
				return IsLessThanOrEqualTo;
			}
			else if (sign == IsGreaterThan)
			{
				return IsLessThan;
			}
			else if (sign == IsLessThanOrEqualTo)
			{
				return IsGreaterThanOrEqualTo;
			}
			else if (sign == IsLessThan)
			{
				return IsGreaterThan;
			}
			else
			{
				return sign;
			}
		}

		public static Boolean CanBeReverted(this IConcept sign)
		{
			ensureSuits(sign);

			return sign != IsEqualTo && sign != IsNotEqualTo;
		}

		public static IConcept CompareThreeValues(IConcept firstSign, IConcept secondSign)
		{
			if (firstSign == IsEqualTo)
			{
				return secondSign;
			}
			else if (secondSign == IsEqualTo)
			{
				return firstSign;
			}
			else if ((secondSign == IsGreaterThan || secondSign == IsGreaterThanOrEqualTo) && (firstSign == IsGreaterThan || firstSign == IsGreaterThanOrEqualTo))
			{
				return (secondSign == IsGreaterThanOrEqualTo && firstSign == IsGreaterThanOrEqualTo) ? IsGreaterThanOrEqualTo : IsGreaterThan;
			}
			else if ((secondSign == IsLessThan || secondSign == IsLessThanOrEqualTo) && (firstSign == IsLessThan || firstSign == IsLessThanOrEqualTo))
			{
				return (secondSign == IsLessThanOrEqualTo && firstSign == IsLessThanOrEqualTo) ? IsLessThanOrEqualTo : IsLessThan;
			}
			else
			{
				return null;
			}
		}

		static ComparisonSigns()
		{
			foreach (var concept in All)
			{
				concept.Attributes.Add(IsValueAttribute.Value);
			}
		}
	}

	public static class SequenceSigns
	{
		#region Properties

		public static readonly IConcept StartsAfterOtherStarted = new Concept(
			new LocalizedStringConstant(lang => lang.SystemConceptNames.StartsAfterOtherStarted),
			new LocalizedStringConstant(lang => lang.SystemConceptHints.StartsAfterOtherStarted))
		{
			Attributes = { IsSequenceSignAttribute.Value },
		};

		public static readonly IConcept StartsWhenOtherStarted = new Concept(
			new LocalizedStringConstant(lang => lang.SystemConceptNames.StartsWhenOtherStarted),
			new LocalizedStringConstant(lang => lang.SystemConceptHints.StartsWhenOtherStarted))
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

		public static readonly IConcept FinishesWhenOtherStarted = new Concept(
			new LocalizedStringConstant(lang => lang.SystemConceptNames.FinishesWhenOtherStarted),
			new LocalizedStringConstant(lang => lang.SystemConceptHints.FinishesWhenOtherStarted))
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

		public static readonly IConcept StartsWhenOtherFinished = new Concept(
			new LocalizedStringConstant(lang => lang.SystemConceptNames.StartsWhenOtherFinished),
			new LocalizedStringConstant(lang => lang.SystemConceptHints.StartsWhenOtherFinished))
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

		public static readonly IConcept FinishesWhenOtherFinished = new Concept(
			new LocalizedStringConstant(lang => lang.SystemConceptNames.FinishesWhenOtherFinished),
			new LocalizedStringConstant(lang => lang.SystemConceptHints.FinishesWhenOtherFinished))
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

		public static readonly IConcept IsCausedBy = new Concept(
			new LocalizedStringConstant(lang => lang.SystemConceptNames.IsCausedBy),
			new LocalizedStringConstant(lang => lang.SystemConceptHints.IsCausedBy))
		{
			Attributes = { IsSequenceSignAttribute.Value },
		};

		public static readonly IConcept SimultaneousWith = new Concept(
			new LocalizedStringConstant(lang => lang.SystemConceptNames.SimultaneousWith),
			new LocalizedStringConstant(lang => lang.SystemConceptHints.SimultaneousWith))
		{
			Attributes = { IsSequenceSignAttribute.Value },
		};

		public static readonly ICollection<IConcept> All = new HashSet<IConcept>
		{
			StartsAfterOtherStarted,
			StartsWhenOtherStarted,
			StartsBeforeOtherStarted,
			FinishesAfterOtherStarted,
			FinishesWhenOtherStarted,
			FinishesBeforeOtherStarted,
			StartsAfterOtherFinished,
			StartsWhenOtherFinished,
			StartsBeforeOtherFinished,
			FinishesAfterOtherFinished,
			FinishesWhenOtherFinished,
			FinishesBeforeOtherFinished,
			Causes,
			IsCausedBy,
			SimultaneousWith,
		};

		public static readonly ICollection<IConcept> StartSigns = new HashSet<IConcept>
		{
			StartsAfterOtherStarted,
			StartsWhenOtherStarted,
			StartsBeforeOtherStarted,
			StartsAfterOtherFinished,
			StartsWhenOtherFinished,
			StartsBeforeOtherFinished,
		};

		public static readonly ICollection<IConcept> FinishSigns = new HashSet<IConcept>
		{
			FinishesAfterOtherFinished,
			FinishesWhenOtherFinished,
			FinishesBeforeOtherFinished,
			FinishesAfterOtherStarted,
			FinishesWhenOtherStarted,
			FinishesBeforeOtherStarted,
		};

		public static readonly ICollection<IConcept> RelatedToStartSigns = new HashSet<IConcept>
		{
			StartsAfterOtherStarted,
			StartsWhenOtherStarted,
			StartsBeforeOtherStarted,
			FinishesAfterOtherStarted,
			FinishesWhenOtherStarted,
			FinishesBeforeOtherStarted,
		};

		public static readonly ICollection<IConcept> RelatedToFinishSigns = new HashSet<IConcept>
		{
			StartsAfterOtherFinished,
			StartsWhenOtherFinished,
			StartsBeforeOtherFinished,
			FinishesAfterOtherFinished,
			FinishesWhenOtherFinished,
			FinishesBeforeOtherFinished,
		};

		public static readonly ICollection<IConcept> AfterSigns = new HashSet<IConcept>
		{
			StartsAfterOtherStarted,
			FinishesAfterOtherStarted,
			StartsAfterOtherFinished,
			FinishesAfterOtherFinished,
		};

		public static readonly ICollection<IConcept> WhenSigns = new HashSet<IConcept>
		{
			StartsWhenOtherStarted,
			FinishesWhenOtherStarted,
			StartsWhenOtherFinished,
			FinishesWhenOtherFinished,
		};

		public static readonly ICollection<IConcept> BeforeSigns = new HashSet<IConcept>
		{
			StartsBeforeOtherStarted,
			FinishesBeforeOtherStarted,
			StartsBeforeOtherFinished,
			FinishesBeforeOtherFinished,
		};

		public static readonly IDictionary<IConcept, IDictionary<IConcept, IConcept>> ValidSequenceCombinations;

		#endregion

		private static void ensureSuits(this IConcept sign)
		{
			if (!All.Contains(sign))
			{
				throw new InvalidOperationException("This method can work only with process sequence signs.");
			}
		}

		public static IConcept Revert(this IConcept sign)
		{
			ensureSuits(sign);

			if (sign == StartsAfterOtherStarted)
			{
				return StartsBeforeOtherStarted;
			}
			else if (sign == StartsBeforeOtherStarted)
			{
				return StartsAfterOtherStarted;
			}
			else if (sign == FinishesAfterOtherStarted)
			{
				return StartsBeforeOtherFinished;
			}
			else if (sign == FinishesWhenOtherStarted)
			{
				return StartsWhenOtherFinished;
			}
			else if (sign == FinishesBeforeOtherStarted)
			{
				return StartsAfterOtherFinished;
			}
			else if (sign == StartsAfterOtherFinished)
			{
				return FinishesBeforeOtherStarted;
			}
			else if (sign == StartsWhenOtherFinished)
			{
				return FinishesWhenOtherStarted;
			}
			else if (sign == StartsBeforeOtherFinished)
			{
				return FinishesAfterOtherStarted;
			}
			else if (sign == FinishesAfterOtherFinished)
			{
				return FinishesBeforeOtherFinished;
			}
			else if (sign == FinishesBeforeOtherFinished)
			{
				return FinishesAfterOtherFinished;
			}
			else if (sign == Causes)
			{
				return IsCausedBy;
			}
			else if (sign == IsCausedBy)
			{
				return Causes;
			}
			else
			{
				return sign;
			}
		}

		public static IConcept TryToCombineMutualSequences(IConcept transitiveSign, IConcept childSign)
		{
			IDictionary<IConcept, IConcept> d;
			IConcept resultSign;
			return ValidSequenceCombinations.TryGetValue(transitiveSign, out d) && d.TryGetValue(childSign, out resultSign)
				? resultSign
				: null;
		}

		static SequenceSigns()
		{
			foreach (var concept in All)
			{
				concept.Attributes.Add(IsValueAttribute.Value);
			}

			ValidSequenceCombinations = new Dictionary<IConcept, IDictionary<IConcept, IConcept>>();

			Action<IConcept, IConcept, IConcept> setValidCombination = (transitiveSign, childSign, resultSign) =>
			{
				IDictionary<IConcept, IConcept> d;
				if (!ValidSequenceCombinations.TryGetValue(transitiveSign, out d))
				{
					ValidSequenceCombinations[transitiveSign] = d = new Dictionary<IConcept, IConcept>();
				}
				d.Add(childSign, resultSign);
			};

			foreach (var combination in new[]
			{
				new Tuple<IConcept, IConcept, IConcept>(StartsAfterOtherStarted, StartsAfterOtherStarted, StartsAfterOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(StartsAfterOtherStarted, StartsWhenOtherStarted, StartsAfterOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(StartsAfterOtherStarted, StartsAfterOtherFinished, StartsAfterOtherFinished),
				new Tuple<IConcept, IConcept, IConcept>(StartsAfterOtherStarted, StartsWhenOtherFinished, StartsAfterOtherFinished),

				new Tuple<IConcept, IConcept, IConcept>(StartsWhenOtherStarted, StartsAfterOtherStarted, StartsAfterOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(StartsWhenOtherStarted, StartsWhenOtherStarted, StartsWhenOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(StartsWhenOtherStarted, StartsBeforeOtherStarted, StartsBeforeOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(StartsWhenOtherStarted, StartsAfterOtherFinished, StartsAfterOtherFinished),
				new Tuple<IConcept, IConcept, IConcept>(StartsWhenOtherStarted, StartsWhenOtherFinished, StartsWhenOtherFinished),
				new Tuple<IConcept, IConcept, IConcept>(StartsWhenOtherStarted, StartsBeforeOtherFinished, StartsBeforeOtherFinished),

				new Tuple<IConcept, IConcept, IConcept>(StartsBeforeOtherStarted, StartsWhenOtherStarted, StartsBeforeOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(StartsBeforeOtherStarted, StartsBeforeOtherStarted, StartsBeforeOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(StartsBeforeOtherStarted, StartsWhenOtherFinished, StartsBeforeOtherFinished),
				new Tuple<IConcept, IConcept, IConcept>(StartsBeforeOtherStarted, StartsBeforeOtherFinished, StartsBeforeOtherFinished),

				new Tuple<IConcept, IConcept, IConcept>(FinishesAfterOtherStarted, StartsAfterOtherStarted, FinishesAfterOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(FinishesAfterOtherStarted, StartsWhenOtherStarted, FinishesAfterOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(FinishesAfterOtherStarted, StartsAfterOtherFinished, FinishesAfterOtherFinished),
				new Tuple<IConcept, IConcept, IConcept>(FinishesAfterOtherStarted, StartsWhenOtherFinished, FinishesAfterOtherFinished),

				new Tuple<IConcept, IConcept, IConcept>(FinishesWhenOtherStarted, StartsAfterOtherStarted, FinishesAfterOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(FinishesWhenOtherStarted, StartsWhenOtherStarted, FinishesWhenOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(FinishesWhenOtherStarted, StartsBeforeOtherStarted, FinishesBeforeOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(FinishesWhenOtherStarted, StartsAfterOtherFinished, FinishesAfterOtherFinished),
				new Tuple<IConcept, IConcept, IConcept>(FinishesWhenOtherStarted, StartsWhenOtherFinished, FinishesWhenOtherFinished),
				new Tuple<IConcept, IConcept, IConcept>(FinishesWhenOtherStarted, StartsBeforeOtherFinished, FinishesBeforeOtherFinished),

				new Tuple<IConcept, IConcept, IConcept>(FinishesBeforeOtherStarted, StartsWhenOtherStarted, FinishesBeforeOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(FinishesBeforeOtherStarted, StartsBeforeOtherStarted, FinishesBeforeOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(FinishesBeforeOtherStarted, StartsWhenOtherFinished, FinishesBeforeOtherFinished),
				new Tuple<IConcept, IConcept, IConcept>(FinishesBeforeOtherStarted, StartsBeforeOtherFinished, FinishesBeforeOtherFinished),

				new Tuple<IConcept, IConcept, IConcept>(StartsAfterOtherFinished, FinishesAfterOtherStarted, StartsAfterOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(StartsAfterOtherFinished, FinishesWhenOtherStarted, StartsAfterOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(StartsAfterOtherFinished, FinishesAfterOtherFinished, StartsAfterOtherFinished),
				new Tuple<IConcept, IConcept, IConcept>(StartsAfterOtherFinished, FinishesWhenOtherFinished, StartsAfterOtherFinished),

				new Tuple<IConcept, IConcept, IConcept>(StartsWhenOtherFinished, FinishesAfterOtherStarted, StartsAfterOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(StartsWhenOtherFinished, FinishesWhenOtherStarted, StartsWhenOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(StartsWhenOtherFinished, FinishesBeforeOtherStarted, StartsBeforeOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(StartsWhenOtherFinished, FinishesAfterOtherFinished, StartsAfterOtherFinished),
				new Tuple<IConcept, IConcept, IConcept>(StartsWhenOtherFinished, FinishesWhenOtherFinished, StartsWhenOtherFinished),
				new Tuple<IConcept, IConcept, IConcept>(StartsWhenOtherFinished, FinishesBeforeOtherFinished, StartsBeforeOtherFinished),

				new Tuple<IConcept, IConcept, IConcept>(StartsBeforeOtherFinished, FinishesWhenOtherStarted, StartsBeforeOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(StartsBeforeOtherFinished, FinishesBeforeOtherStarted, StartsBeforeOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(StartsBeforeOtherFinished, FinishesWhenOtherFinished, StartsBeforeOtherFinished),
				new Tuple<IConcept, IConcept, IConcept>(StartsBeforeOtherFinished, FinishesBeforeOtherFinished, StartsBeforeOtherFinished),

				new Tuple<IConcept, IConcept, IConcept>(FinishesAfterOtherFinished, FinishesAfterOtherStarted, FinishesAfterOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(FinishesAfterOtherFinished, FinishesWhenOtherStarted, FinishesAfterOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(FinishesAfterOtherFinished, FinishesAfterOtherFinished, FinishesAfterOtherFinished),
				new Tuple<IConcept, IConcept, IConcept>(FinishesAfterOtherFinished, FinishesWhenOtherFinished, FinishesAfterOtherFinished),

				new Tuple<IConcept, IConcept, IConcept>(FinishesWhenOtherFinished, FinishesAfterOtherStarted, FinishesAfterOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(FinishesWhenOtherFinished, FinishesWhenOtherStarted, FinishesWhenOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(FinishesWhenOtherFinished, FinishesBeforeOtherStarted, FinishesBeforeOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(FinishesWhenOtherFinished, FinishesAfterOtherFinished, FinishesAfterOtherFinished),
				new Tuple<IConcept, IConcept, IConcept>(FinishesWhenOtherFinished, FinishesWhenOtherFinished, FinishesWhenOtherFinished),
				new Tuple<IConcept, IConcept, IConcept>(FinishesWhenOtherFinished, FinishesBeforeOtherFinished, FinishesBeforeOtherFinished),

				new Tuple<IConcept, IConcept, IConcept>(FinishesBeforeOtherFinished, FinishesWhenOtherStarted, FinishesBeforeOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(FinishesBeforeOtherFinished, FinishesBeforeOtherStarted, FinishesBeforeOtherStarted),
				new Tuple<IConcept, IConcept, IConcept>(FinishesBeforeOtherFinished, FinishesWhenOtherFinished, FinishesBeforeOtherFinished),
				new Tuple<IConcept, IConcept, IConcept>(FinishesBeforeOtherFinished, FinishesBeforeOtherFinished, FinishesBeforeOtherFinished),

				new Tuple<IConcept, IConcept, IConcept>(Causes, Causes, Causes),
				new Tuple<IConcept, IConcept, IConcept>(IsCausedBy, IsCausedBy, IsCausedBy),
				new Tuple<IConcept, IConcept, IConcept>(SimultaneousWith, SimultaneousWith, SimultaneousWith),
			})
			{
				setValidCombination(combination.Item1, combination.Item2, combination.Item3);
			}
		}
	}

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
