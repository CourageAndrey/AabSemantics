using System;
using System.Collections.Generic;
using System.Linq;

namespace Inventor.Core.Statements
{
	public class ComparisonStatementContradictionsChecker : ContradictionsChecker<ComparisonStatement>
	{
		public ComparisonStatementContradictionsChecker(IEnumerable<ComparisonStatement> statements)
			: base(statements)
		{
			makeAllValuesAlwaysEqualToThemselves();
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

		protected override Boolean SetCombinationWithDescendants(IConcept valueRow, IConcept valueColumn, IConcept sign)
		{
			Boolean combinationsUpdated = SetCombination(valueRow, valueColumn, sign);
			if (sign.CanBeReverted())
			{
				combinationsUpdated |= SetCombination(valueColumn, valueRow, ComparisonSigns.Revert(sign));
			}
			return combinationsUpdated;
		}

		protected override Boolean Contradicts(HashSet<IConcept> signs, IConcept left, IConcept right)
		{
			return doesOneOrMoreContradictedSignsPairExist(signs) || doesValueContradictToItself(signs, left, right);
		}

		private void makeAllValuesAlwaysEqualToThemselves()
		{
			foreach (var value in AllValues)
			{
				SetCombination(value, value, ComparisonSigns.IsEqualTo);
			}
		}

		private static Boolean doesOneOrMoreContradictedSignsPairExist(ICollection<IConcept> signs)
		{
			foreach (var sign1 in signs)
			{
				foreach (var sign2 in signs)
				{
					if (sign1.Contradicts(sign2))
					{
						return true;
					}
				}
			}
			return false;
		}

		private static Boolean doesValueContradictToItself(HashSet<IConcept> signs, IConcept left, IConcept right)
		{
			return left == right && signs.Any(s => s != ComparisonSigns.IsEqualTo);
		}

		protected override Boolean TryToUpdateCombinations(IConcept valueRow, IConcept signRow, IConcept signColumn, IConcept valueColumn)
		{
			var resultSign = ComparisonSigns.CompareThreeValues(signRow, signColumn);
			return resultSign != null && SetCombinationWithDescendants(valueRow, valueColumn, resultSign);
		}
	}
}