﻿using System;
using System.Collections.Generic;
using System.Linq;

using AabSemantics.Concepts;
using AabSemantics.Localization;
using AabSemantics.Modules.Boolean.Attributes;
using AabSemantics.Modules.Mathematics.Attributes;
using AabSemantics.Modules.Mathematics.Localization;

namespace AabSemantics.Modules.Mathematics.Concepts
{
	public static class ComparisonSigns
	{
		#region Properties

		public static readonly IConcept IsEqualTo = new SystemConcept(
			$"{{{nameof(ComparisonSigns)}.{nameof(IsEqualTo)}}}",
			new LocalizedStringConstant(lang => lang.GetConceptsExtension<ILanguageMathematicsModule, ILanguageConcepts>().SystemConceptNames.IsEqualTo),
			new LocalizedStringConstant(lang => lang.GetConceptsExtension<ILanguageMathematicsModule, ILanguageConcepts>().SystemConceptHints.IsEqualTo));

		public static readonly IConcept IsNotEqualTo = new SystemConcept(
			$"{{{nameof(ComparisonSigns)}.{nameof(IsNotEqualTo)}}}",
			new LocalizedStringConstant(lang => lang.GetConceptsExtension<ILanguageMathematicsModule, ILanguageConcepts>().SystemConceptNames.IsNotEqualTo),
			new LocalizedStringConstant(lang => lang.GetConceptsExtension<ILanguageMathematicsModule, ILanguageConcepts>().SystemConceptHints.IsNotEqualTo));

		public static readonly IConcept IsGreaterThanOrEqualTo = new SystemConcept(
			$"{{{nameof(ComparisonSigns)}.{nameof(IsGreaterThanOrEqualTo)}}}",
			new LocalizedStringConstant(lang => lang.GetConceptsExtension<ILanguageMathematicsModule, ILanguageConcepts>().SystemConceptNames.IsGreaterThanOrEqualTo),
			new LocalizedStringConstant(lang => lang.GetConceptsExtension<ILanguageMathematicsModule, ILanguageConcepts>().SystemConceptHints.IsGreaterThanOrEqualTo));

		public static readonly IConcept IsGreaterThan = new SystemConcept(
			$"{{{nameof(ComparisonSigns)}.{nameof(IsGreaterThan)}}}",
			new LocalizedStringConstant(lang => lang.GetConceptsExtension<ILanguageMathematicsModule, ILanguageConcepts>().SystemConceptNames.IsGreaterThan),
			new LocalizedStringConstant(lang => lang.GetConceptsExtension<ILanguageMathematicsModule, ILanguageConcepts>().SystemConceptHints.IsGreaterThan));

		public static readonly IConcept IsLessThanOrEqualTo = new SystemConcept(
			$"{{{nameof(ComparisonSigns)}.{nameof(IsLessThanOrEqualTo)}}}",
			new LocalizedStringConstant(lang => lang.GetConceptsExtension<ILanguageMathematicsModule, ILanguageConcepts>().SystemConceptNames.IsLessThanOrEqualTo),
			new LocalizedStringConstant(lang => lang.GetConceptsExtension<ILanguageMathematicsModule, ILanguageConcepts>().SystemConceptHints.IsLessThanOrEqualTo));

		public static readonly IConcept IsLessThan = new SystemConcept(
			$"{{{nameof(ComparisonSigns)}.{nameof(IsLessThan)}}}",
			new LocalizedStringConstant(lang => lang.GetConceptsExtension<ILanguageMathematicsModule, ILanguageConcepts>().SystemConceptNames.IsLessThan),
			new LocalizedStringConstant(lang => lang.GetConceptsExtension<ILanguageMathematicsModule, ILanguageConcepts>().SystemConceptHints.IsLessThan));

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

		public static System.Boolean Contradicts(this IConcept sign1, IConcept sign2)
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

		public static System.Boolean CanBeReverted(this IConcept sign)
		{
			ensureSuits(sign);

			return sign != IsEqualTo && sign != IsNotEqualTo;
		}

		public static IConcept CompareThreeValues(IConcept firstSign, IConcept secondSign)
		{
			ensureSuits(firstSign);
			ensureSuits(secondSign);

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
				concept.WithAttributes(new IAttribute[] { IsValueAttribute.Value, IsComparisonSignAttribute.Value });
			}
		}
	}
}