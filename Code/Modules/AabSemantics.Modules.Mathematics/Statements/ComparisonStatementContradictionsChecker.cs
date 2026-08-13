using System.Collections.Generic;
using System.Threading.Tasks;

using AabSemantics.Modules.Mathematics.Concepts;
using AabSemantics.Statements;
using AabSemantics.Utils;

namespace AabSemantics.Modules.Mathematics.Statements
{
	/// <summary>
	/// Finds contradictions among comparison statements. On top of the base matrix algorithm it
	/// seeds every value as equal to itself, mirrors each recorded sign into the transposed cell,
	/// and composes signs across a shared value to derive new comparisons.
	/// </summary>
	public class ComparisonStatementContradictionsChecker : ContradictionsChecker<ComparisonStatement>
	{
		/// <summary>Seeds the matrix from the given statements and adds the reflexive equalities.</summary>
		/// <param name="statements">Statements to analyse.</param>
		public ComparisonStatementContradictionsChecker(IEnumerable<ComparisonStatement> statements)
			: base(statements)
		{
			MakeAllValuesAlwaysEqualToThemselves();
		}

		/// <summary>Reads the statement's left-hand value.</summary>
		/// <param name="statement">Statement to read.</param>
		/// <returns>The left value.</returns>
		protected override IConcept GetLeftValue(ComparisonStatement statement)
		{
			return statement.LeftValue;
		}

		/// <summary>Reads the statement's right-hand value.</summary>
		/// <param name="statement">Statement to read.</param>
		/// <returns>The right value.</returns>
		protected override IConcept GetRightValue(ComparisonStatement statement)
		{
			return statement.RightValue;
		}

		/// <summary>Reads the statement's comparison sign.</summary>
		/// <param name="statement">Statement to read.</param>
		/// <returns>The sign.</returns>
		protected override IConcept GetSign(ComparisonStatement statement)
		{
			return statement.ComparisonSign;
		}

		/// <summary>Records a sign and, when the sign is asymmetric, its reverted form in the transposed cell.</summary>
		/// <param name="valueRow">Row value.</param>
		/// <param name="valueColumn">Column value.</param>
		/// <param name="sign">Sign to record.</param>
		/// <returns><c>true</c> if the matrix changed.</returns>
		protected override System.Boolean SetCombinationWithDescendants(IConcept valueRow, IConcept valueColumn, IConcept sign)
		{
			System.Boolean combinationsUpdated = SetCombination(valueRow, valueColumn, sign);
			if (sign.CanBeReverted())
			{
				combinationsUpdated |= SetCombination(valueColumn, valueRow, ComparisonSigns.Revert(sign));
			}
			return combinationsUpdated;
		}

		/// <summary>
		/// Reports a contradiction when the cell holds two mutually exclusive signs, or when a value
		/// ends up related to itself by anything other than equality.
		/// </summary>
		/// <param name="signs">Signs recorded for the pair.</param>
		/// <param name="left">Left value of the pair.</param>
		/// <param name="right">Right value of the pair.</param>
		/// <returns><c>true</c> if the signs cannot hold together.</returns>
		protected override async Task<System.Boolean> ContradictsAsync(HashSet<IConcept> signs, IConcept left, IConcept right)
		{
			return await DoesOneOrMoreContradictedSignsPairExistAsync(signs) || await DoesValueContradictToItselfAsync(signs, left, right);
		}

		private void MakeAllValuesAlwaysEqualToThemselves()
		{
			foreach (var value in AllValues)
			{
				SetCombination(value, value, ComparisonSigns.IsEqualTo);
			}
		}

		private static async Task<System.Boolean> DoesOneOrMoreContradictedSignsPairExistAsync(ICollection<IConcept> signs)
		{
			foreach (var sign1 in signs)
			{
				foreach (var sign2 in signs)
				{
					if (await sign1.ContradictsAsync(sign2))
					{
						return true;
					}
				}
			}
			return false;
		}

		private static async Task<System.Boolean> DoesValueContradictToItselfAsync(HashSet<IConcept> signs, IConcept left, IConcept right)
		{
			return left == right && await signs.AnyAsync(s => s != ComparisonSigns.IsEqualTo);
		}

		/// <summary>Derives the sign between two values from their signs against a shared middle value.</summary>
		/// <param name="valueRow">First value.</param>
		/// <param name="signRow">Sign between it and the middle value.</param>
		/// <param name="signColumn">Sign between the middle value and the second one.</param>
		/// <param name="valueColumn">Second value.</param>
		/// <returns><c>true</c> if a sign followed and the matrix changed.</returns>
		protected override System.Boolean TryToUpdateCombinations(IConcept valueRow, IConcept signRow, IConcept signColumn, IConcept valueColumn)
		{
			var resultSign = ComparisonSigns.CompareThreeValues(signRow, signColumn);
			return resultSign != null && SetCombinationWithDescendants(valueRow, valueColumn, resultSign);
		}
	}
}