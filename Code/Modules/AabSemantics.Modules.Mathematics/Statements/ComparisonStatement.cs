using AabSemantics.Modules.Boolean.Attributes;
using AabSemantics.Modules.Mathematics.Attributes;
using AabSemantics.Modules.Mathematics.Concepts;
using AabSemantics.Modules.Mathematics.Localization;
using AabSemantics.Modules.Mathematics.Questions;
using AabSemantics.Statements;
using AabSemantics.Utils;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AabSemantics.Modules.Mathematics.Statements
{
	/// <summary>
	/// States how two values compare, e.g. "A is greater than B". The operand order matters, so
	/// the same fact can be written two ways; see <see cref="SwapOperands"/>.
	/// </summary>
	public class ComparisonStatement : Statement<ComparisonStatement>
	{
		#region Properties

		/// <summary>The left-hand value; must carry the "is a value" attribute.</summary>
		public IConcept LeftValue
		{ get; private set; }

		/// <summary>The right-hand value; must carry the "is a value" attribute.</summary>
		public IConcept RightValue
		{ get; private set; }

		/// <summary>The relation between them; must carry the "is a comparison sign" attribute.</summary>
		public IConcept ComparisonSign
		{ get; private set; }

		#endregion

		/// <summary>Creates a comparison statement.</summary>
		/// <param name="id">Identifier; a GUID is generated when null or empty.</param>
		/// <param name="leftValue">The left-hand value.</param>
		/// <param name="rightValue">The right-hand value.</param>
		/// <param name="comparisonSign">The relation between them.</param>
		/// <exception cref="ArgumentNullException">Any concept is <c>null</c>.</exception>
		/// <exception cref="ArgumentException">A concept lacks the attribute its role requires.</exception>
		public ComparisonStatement(String id, IConcept leftValue, IConcept rightValue, IConcept comparisonSign)
			: base(
				id,
				new Func<ILanguage, String>(language => language.GetStatementsExtension<ILanguageMathematicsModule, ILanguageStatements>().Names.Comparison),
				new Func<ILanguage, String>(language => language.GetStatementsExtension<ILanguageMathematicsModule, ILanguageStatements>().Hints.Comparison))
		{
			Update(id, leftValue, rightValue, comparisonSign);
		}

		/// <summary>Reassigns the identifier and all three concepts, re-checking their attributes.</summary>
		/// <param name="id">New identifier; a GUID is generated when null or empty.</param>
		/// <param name="leftValue">The left-hand value.</param>
		/// <param name="rightValue">The right-hand value.</param>
		/// <param name="comparisonSign">The relation between them.</param>
		/// <exception cref="ArgumentNullException">Any concept is <c>null</c>.</exception>
		/// <exception cref="ArgumentException">A concept lacks the attribute its role requires.</exception>
		public void Update(String id, IConcept leftValue, IConcept rightValue, IConcept comparisonSign)
		{
			Update(id);
			LeftValue = leftValue.EnsureNotNull(nameof(leftValue)).EnsureHasAttribute<IConcept, IsValueAttribute>(nameof(leftValue));
			RightValue = rightValue.EnsureNotNull(nameof(rightValue)).EnsureHasAttribute<IConcept, IsValueAttribute>(nameof(rightValue));
			ComparisonSign = comparisonSign.EnsureNotNull(nameof(comparisonSign)).EnsureHasAttribute<IConcept, IsComparisonSignAttribute>(nameof(comparisonSign));
		}

		/// <summary>Returns both values and the sign.</summary>
		/// <returns>The left value, the right value and the comparison sign.</returns>
		public override IEnumerable<IConcept> GetChildConcepts()
		{
			yield return LeftValue;
			yield return RightValue;
			yield return ComparisonSign;
		}

		#region Consistency checking

		/// <summary>
		/// Compares all three concepts by reference. Note that a statement and its swapped
		/// equivalent are <em>not</em> equal, even though they assert the same thing.
		/// </summary>
		/// <param name="other">Statement to compare with; may be <c>null</c>.</param>
		/// <returns><c>true</c> if both hold the same values in the same order with the same sign.</returns>
		public override System.Boolean Equals(ComparisonStatement other)
		{
			if (ReferenceEquals(this, other)) return true;
			if (other != null)
			{
				return	other.LeftValue == LeftValue &&
						other.RightValue == RightValue &&
						other.ComparisonSign == ComparisonSign;
			}
			else return false;
		}

		#endregion

		/// <summary>
		/// Returns this statement with its operands ordered the way a question asks about them,
		/// so the answer reads in the expected direction.
		/// </summary>
		/// <param name="question">Question whose operand order should be matched.</param>
		/// <returns>A swapped copy when the orders differ; otherwise this statement itself.</returns>
		public ComparisonStatement SwapOperandsToMatchOrder(ComparisonQuestion question)
		{
			return RightValue == question.LeftValue || LeftValue == question.RightValue
				? SwapOperands()
				: this;
		}

		/// <summary>
		/// Returns the equivalent statement with the operands exchanged and the sign reverted.
		/// The copy gets a fresh identifier and is not added to any network.
		/// </summary>
		/// <returns>A new, equivalent statement.</returns>
		public ComparisonStatement SwapOperands()
		{
			return new ComparisonStatement(null, leftValue: RightValue, rightValue: LeftValue, comparisonSign: ComparisonSigns.Revert(ComparisonSign));
		}
	}

	/// <summary>Consistency checking over a set of comparison statements.</summary>
	public static class ComparisonStatementConsistencyExtension
	{
		/// <summary>Infers everything derivable from the statements and reports the contradictions found.</summary>
		/// <param name="statements">Statements to analyse.</param>
		/// <returns>One entry per contradicting pair of values; empty when the set is consistent.</returns>
		public static async Task<List<Contradiction>> CheckForContradictionsAsync(this IEnumerable<ComparisonStatement> statements)
		{
			var checker = new ComparisonStatementContradictionsChecker(statements);
			return await checker.CheckForContradictionsAsync();
		}

		/// <summary>Blocking counterpart of <see cref="CheckForContradictionsAsync"/>.</summary>
		/// <param name="statements">Statements to analyse.</param>
		/// <returns>One entry per contradicting pair of values.</returns>
		public static List<Contradiction> CheckForContradictions(this IEnumerable<ComparisonStatement> statements)
		{
			return TaskHelper.AwaitDetached(() => CheckForContradictionsAsync(statements));
		}
	}
}
