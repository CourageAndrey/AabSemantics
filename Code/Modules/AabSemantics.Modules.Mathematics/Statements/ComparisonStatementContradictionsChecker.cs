using System.Collections.Generic;
using System.Threading.Tasks;

using AabSemantics.Modules.Mathematics.Concepts;
using AabSemantics.Statements;
using AabSemantics.Utils;

namespace AabSemantics.Modules.Mathematics.Statements
{
	public class ComparisonStatementContradictionsChecker : ContradictionsChecker<ComparisonStatement>
	{
		public ComparisonStatementContradictionsChecker(IEnumerable<ComparisonStatement> statements)
			: base(statements)
		{
			MakeAllValuesAlwaysEqualToThemselves();
		}

		protected override IConcept GetLeftValue(ComparisonStatement statement)
		{
			return statement.LeftValue;
		}

		protected override IConcept GetRightValue(ComparisonStatement statement)
		{
			return statement.RightValue;
		}

		protected override IConcept GetSign(ComparisonStatement statement)
		{
			return statement.ComparisonSign;
		}

		protected override System.Boolean SetCombinationWithDescendants(IConcept valueRow, IConcept valueColumn, IConcept sign)
		{
			System.Boolean combinationsUpdated = SetCombination(valueRow, valueColumn, sign);
			if (sign.CanBeReverted())
			{
				combinationsUpdated |= SetCombination(valueColumn, valueRow, ComparisonSigns.Revert(sign));
			}
			return combinationsUpdated;
		}

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

		protected override System.Boolean TryToUpdateCombinations(IConcept valueRow, IConcept signRow, IConcept signColumn, IConcept valueColumn)
		{
			var resultSign = ComparisonSigns.CompareThreeValues(signRow, signColumn);
			return resultSign != null && SetCombinationWithDescendants(valueRow, valueColumn, resultSign);
		}
	}
}