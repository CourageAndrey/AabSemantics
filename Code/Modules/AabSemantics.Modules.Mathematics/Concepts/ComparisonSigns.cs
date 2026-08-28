using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AabSemantics.Concepts;
using AabSemantics.Localization;
using AabSemantics.Modules.Boolean.Attributes;
using AabSemantics.Modules.Mathematics.Attributes;
using AabSemantics.Modules.Mathematics.Localization;
using AabSemantics.Utils;

namespace AabSemantics.Modules.Mathematics.Concepts
{
	/// <summary>
	/// The six comparison signs as concepts, plus the algebra over them: which pairs contradict,
	/// how a sign reverts when the operands swap, and how two signs compose across a shared value.
	/// Each sign is a system concept carrying both <see cref="IsValueAttribute"/> and
	/// <see cref="IsComparisonSignAttribute"/>.
	/// </summary>
	public static class ComparisonSigns
	{
		#region Properties

		/// <summary>The "equal to" comparison sign.</summary>
		public static readonly IConcept IsEqualTo = new SystemConcept(
			$"{{{nameof(ComparisonSigns)}.{nameof(IsEqualTo)}}}",
			new LocalizedStringConstant(lang => lang.GetConceptsExtension<ILanguageMathematicsModule, ILanguageConcepts>().SystemConceptNames.IsEqualTo),
			new LocalizedStringConstant(lang => lang.GetConceptsExtension<ILanguageMathematicsModule, ILanguageConcepts>().SystemConceptHints.IsEqualTo));

		/// <summary>The "not equal to" comparison sign.</summary>
		public static readonly IConcept IsNotEqualTo = new SystemConcept(
			$"{{{nameof(ComparisonSigns)}.{nameof(IsNotEqualTo)}}}",
			new LocalizedStringConstant(lang => lang.GetConceptsExtension<ILanguageMathematicsModule, ILanguageConcepts>().SystemConceptNames.IsNotEqualTo),
			new LocalizedStringConstant(lang => lang.GetConceptsExtension<ILanguageMathematicsModule, ILanguageConcepts>().SystemConceptHints.IsNotEqualTo));

		/// <summary>The "greater than or equal to" comparison sign.</summary>
		public static readonly IConcept IsGreaterThanOrEqualTo = new SystemConcept(
			$"{{{nameof(ComparisonSigns)}.{nameof(IsGreaterThanOrEqualTo)}}}",
			new LocalizedStringConstant(lang => lang.GetConceptsExtension<ILanguageMathematicsModule, ILanguageConcepts>().SystemConceptNames.IsGreaterThanOrEqualTo),
			new LocalizedStringConstant(lang => lang.GetConceptsExtension<ILanguageMathematicsModule, ILanguageConcepts>().SystemConceptHints.IsGreaterThanOrEqualTo));

		/// <summary>The "greater than" comparison sign.</summary>
		public static readonly IConcept IsGreaterThan = new SystemConcept(
			$"{{{nameof(ComparisonSigns)}.{nameof(IsGreaterThan)}}}",
			new LocalizedStringConstant(lang => lang.GetConceptsExtension<ILanguageMathematicsModule, ILanguageConcepts>().SystemConceptNames.IsGreaterThan),
			new LocalizedStringConstant(lang => lang.GetConceptsExtension<ILanguageMathematicsModule, ILanguageConcepts>().SystemConceptHints.IsGreaterThan));

		/// <summary>The "less than or equal to" comparison sign.</summary>
		public static readonly IConcept IsLessThanOrEqualTo = new SystemConcept(
			$"{{{nameof(ComparisonSigns)}.{nameof(IsLessThanOrEqualTo)}}}",
			new LocalizedStringConstant(lang => lang.GetConceptsExtension<ILanguageMathematicsModule, ILanguageConcepts>().SystemConceptNames.IsLessThanOrEqualTo),
			new LocalizedStringConstant(lang => lang.GetConceptsExtension<ILanguageMathematicsModule, ILanguageConcepts>().SystemConceptHints.IsLessThanOrEqualTo));

		/// <summary>The "less than" comparison sign.</summary>
		public static readonly IConcept IsLessThan = new SystemConcept(
			$"{{{nameof(ComparisonSigns)}.{nameof(IsLessThan)}}}",
			new LocalizedStringConstant(lang => lang.GetConceptsExtension<ILanguageMathematicsModule, ILanguageConcepts>().SystemConceptNames.IsLessThan),
			new LocalizedStringConstant(lang => lang.GetConceptsExtension<ILanguageMathematicsModule, ILanguageConcepts>().SystemConceptHints.IsLessThan));

		/// <summary>All six signs; every helper below rejects concepts outside this set.</summary>
		public static readonly ICollection<IConcept> All = new HashSet<IConcept>
		{
			IsEqualTo,
			IsNotEqualTo,
			IsGreaterThanOrEqualTo,
			IsGreaterThan,
			IsLessThanOrEqualTo,
			IsLessThan,
		};

		/// <summary>Sign pairs that cannot hold for the same two values. Order-insensitive when tested through <see cref="Contradicts"/>.</summary>
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

		/// <summary>Determines whether two signs contradict each other, in either order.</summary>
		/// <param name="sign1">First sign.</param>
		/// <param name="sign2">Second sign.</param>
		/// <returns><c>true</c> if the two cannot both hold.</returns>
		/// <exception cref="InvalidOperationException">Either argument is not a comparison sign.</exception>
		public static System.Boolean Contradicts(this IConcept sign1, IConcept sign2)
		{
			EnsureSuits(sign1);
			EnsureSuits(sign2);

			return Contradictions.Any(tuple =>
				tuple.Item1 == sign2 && tuple.Item2 == sign1 ||
				tuple.Item1 == sign1 && tuple.Item2 == sign2);
		}

		/// <summary>Asynchronous counterpart of <see cref="Contradicts"/>.</summary>
		/// <param name="sign1">First sign.</param>
		/// <param name="sign2">Second sign.</param>
		/// <returns><c>true</c> if the two cannot both hold.</returns>
		/// <exception cref="InvalidOperationException">Either argument is not a comparison sign.</exception>
		public static Task<System.Boolean> ContradictsAsync(this IConcept sign1, IConcept sign2)
		{
			return TaskHelper.FromSynchronous(() => sign1.Contradicts(sign2));
		}

		private static void EnsureSuits(this IConcept sign)
		{
			if (!All.Contains(sign))
			{
				throw new InvalidOperationException("This method can work only with comparison signs.");
			}
		}

		/// <summary>Returns the sign that holds when the two compared values swap places.</summary>
		/// <param name="sign">Sign to revert.</param>
		/// <returns>The mirrored sign; the symmetric signs are returned unchanged.</returns>
		/// <exception cref="InvalidOperationException"><paramref name="sign"/> is not a comparison sign.</exception>
		public static IConcept Revert(this IConcept sign)
		{
			EnsureSuits(sign);

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

		/// <summary>Reports whether reverting the sign actually changes it.</summary>
		/// <param name="sign">Sign to test.</param>
		/// <returns><c>false</c> for the symmetric signs, equal and not-equal.</returns>
		/// <exception cref="InvalidOperationException"><paramref name="sign"/> is not a comparison sign.</exception>
		public static System.Boolean CanBeReverted(this IConcept sign)
		{
			EnsureSuits(sign);

			return sign != IsEqualTo && sign != IsNotEqualTo;
		}

		/// <summary>
		/// Composes two comparisons sharing a middle value into one comparison of the outer values,
		/// which is how the module derives new comparisons transitively.
		/// </summary>
		/// <param name="firstSign">Sign relating the first value to the middle one.</param>
		/// <param name="secondSign">Sign relating the middle value to the last one.</param>
		/// <returns>
		/// The derived sign, or <c>null</c> when nothing follows — for instance from
		/// "greater than" combined with "less than", or from any pair involving not-equal.
		/// </returns>
		/// <exception cref="InvalidOperationException">Either argument is not a comparison sign.</exception>
		public static IConcept CompareThreeValues(IConcept firstSign, IConcept secondSign)
		{
			EnsureSuits(firstSign);
			EnsureSuits(secondSign);

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