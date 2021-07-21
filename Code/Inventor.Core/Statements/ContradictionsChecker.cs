using System;
using System.Collections.Generic;
using System.Linq;

namespace Inventor.Core.Statements
{
	public abstract class ContradictionsChecker<StatementT>
		where StatementT : IStatement
	{
		private readonly HashSet<IConcept> _allValues; // all unique involved values
		private readonly Dictionary<IConcept, Dictionary<IConcept, HashSet<IConcept>>> _allSigns; // matrix of known signs

		protected ContradictionsChecker(IEnumerable<StatementT> statements)
		{
			_allValues = new HashSet<IConcept>();
			_allSigns = new Dictionary<IConcept, Dictionary<IConcept, HashSet<IConcept>>>();

			foreach (var statement in statements)
			{
				var leftValue = GetLeftValue(statement);
				var rightValue = GetRightValue(statement);
				var sign = GetSign(statement);

				_allValues.Add(leftValue);
				_allValues.Add(rightValue);

				setCombination(leftValue, rightValue, sign);
				if (canBeReverted(sign))
				{
					setCombination(rightValue, leftValue, sign.Revert());
				}
			}
		}

		protected abstract IConcept GetLeftValue(StatementT statement);

		protected abstract IConcept GetRightValue(StatementT statement);

		protected abstract IConcept GetSign(StatementT statement);

		public List<Contradiction> CheckForContradictions()
		{
			makeAllValuesAlwaysEqualToThemselves();

			while (updateInferredCombinations())
			{ }

			return findContradictionsInMatrix();
		}

		private static Boolean canBeReverted(IConcept sign)
		{
			return sign != SystemConcepts.IsEqualTo && sign != SystemConcepts.IsNotEqualTo;
		}

		private void makeAllValuesAlwaysEqualToThemselves()
		{
			foreach (var value in _allValues)
			{
				setCombination(value, value, SystemConcepts.IsEqualTo);
			}
		}

		private Boolean updateInferredCombinations()
		{
			Boolean combinationsUpdated = false;
			foreach (var row in _allValues)
			{
				foreach (var column in _allValues)
				{
					if (row != column)
					{
						combinationsUpdated |= updateInferredCombinationsFromCell(row, column);
					}
				}
			}
			return combinationsUpdated;
		}

		private Boolean updateInferredCombinationsFromCell(IConcept row, IConcept column)
		{
			Dictionary<IConcept, HashSet<IConcept>> combinationsRow;
			HashSet<IConcept> signsRow;
			if (_allSigns.TryGetValue(row, out combinationsRow) && combinationsRow.TryGetValue(column, out signsRow)) // if value in current cell is set
			{
				Dictionary<IConcept, HashSet<IConcept>> combinationsColumn;
				if (_allSigns.TryGetValue(column, out combinationsColumn)) // if current value has comparisons with other values
				{
					return updateAllInferredCombinationsWithinCell(row, combinationsColumn, signsRow);
				}
			}
			return false;
		}

		private Boolean updateAllInferredCombinationsWithinCell(
			IConcept row,
			Dictionary<IConcept, HashSet<IConcept>> combinationsColumn,
			HashSet<IConcept> signsRow)
		{
			Boolean combinationsUpdated = false;
			foreach (var kvp in combinationsColumn)
			{
				var valueColumn = kvp.Key;
				var signsColumn = kvp.Value;

				foreach (var signRow in signsRow.ToList())
				{
					foreach (var signColumn in signsColumn.ToList())
					{
						combinationsUpdated |= tryToUpdateCombinations(row, signRow, signColumn, valueColumn);
					}
				}
			}
			return combinationsUpdated;
		}

		private Boolean tryToUpdateCombinations(
			IConcept row,
			IConcept signRow,
			IConcept signColumn,
			IConcept valueColumn)
		{
			Boolean combinationsUpdated = false;

			var resultSign = SystemConcepts.CompareThreeValues(signRow, signColumn);
			if (resultSign != null)
			{
				combinationsUpdated |= setCombination(row, valueColumn, resultSign);
				if (canBeReverted(resultSign))
				{
					combinationsUpdated |= setCombination(valueColumn, row, resultSign.Revert());
				}
			}

			return combinationsUpdated;
		}

		private List<Contradiction> findContradictionsInMatrix()
		{
			var foundContradictions = new List<Contradiction>();
			foreach (var leftCombinations in _allSigns)
			{
				var left = leftCombinations.Key;
				foreach (var rightCombinations in leftCombinations.Value)
				{
					var right = rightCombinations.Key;
					var signs = rightCombinations.Value;
					findContradictionsInCell(signs, left, right, foundContradictions);
				}
			}
			return foundContradictions;
		}

		private Boolean setCombination(IConcept left, IConcept right, IConcept sign)
		{
			Boolean updated = false;

			// get "row" using LEFT value as key, return true if row is added
			Dictionary<IConcept, HashSet<IConcept>> combinations;
			if (!_allSigns.TryGetValue(left, out combinations))
			{
				_allSigns[left] = combinations = new Dictionary<IConcept, HashSet<IConcept>>();
				updated = true;
			}

			// get "column" using RIGHT value as key, return true if column is added
			HashSet<IConcept> signs;
			if (!combinations.TryGetValue(right, out signs))
			{
				combinations[right] = signs = new HashSet<IConcept>();
				updated = true;
			}

			// add value to list, return true if added (= new unique)
			Int32 countBefore = signs.Count;
			signs.Add(sign);
			if (signs.Count > countBefore)
			{
				updated = true;
			}

			return updated;
		}

		/*private string display()
		{
			var align = new Func<string, int, string>((text, lenght) => text.PadLeft(lenght, ' '));

			var signSymbols = new Dictionary<IConcept, string>
			{
				{ SystemConcepts.IsEqualTo, "=" },
				{ SystemConcepts.IsNotEqualTo, "≠" },
				{ SystemConcepts.IsGreaterThanOrEqualTo, "≥" },
				{ SystemConcepts.IsGreaterThan, ">" },
				{ SystemConcepts.IsLessThanOrEqualTo, "≤" },
				{ SystemConcepts.IsLessThan, "<" },
			};

			var headers = _allValues.ToDictionary(
				value => value,
				value => value.Name.GetValue(Localization.Language.Default));
			int headersMaxLength = headers.Values.Max(h => h.Length);

			int signsMaxCount = int.MinValue;
			foreach (var dictionary in _allSigns.Values)
			{
				signsMaxCount = Math.Max(signsMaxCount, dictionary.Values.Max(list => list.Count));
			}

			int columnWidth = Math.Max(headersMaxLength, signsMaxCount);
			string columnsHeader = string.Join("|", headers.Values.Select(h => align(h, columnWidth)));
			string tableHeader = new string(' ', headersMaxLength) + "|" + columnsHeader;
			string afterHeaderLine = new string(tableHeader.Select(c => c == '|' ? '+' : '-').ToArray());
			string emptyLine = new string(columnsHeader.Select(c => c == '|' ? '|' : ' ').ToArray());
			string emptyCell = new string(' ', columnWidth);

			var matrix = new System.Text.StringBuilder();
			matrix.AppendLine(tableHeader);
			matrix.AppendLine(afterHeaderLine);

			foreach (var value1 in _allValues)
			{
				matrix.Append(align(headers[value1], headersMaxLength));
				matrix.Append("|");

				Dictionary<IConcept, HashSet<IConcept>> row;
				if (_allSigns.TryGetValue(value1, out row))
				{
					foreach (var value2 in _allValues)
					{
						HashSet<IConcept> cellValue;
						if (row.TryGetValue(value2, out cellValue))
						{
							matrix.Append(align(string.Join(string.Empty, cellValue.Select(s => signSymbols[s])), columnWidth));
						}
						else
						{
							matrix.Append(emptyCell);
						}

						if (_allValues.Last() != value2)
						{
							matrix.Append("|");
						}
					}

					if (_allValues.Last() != value1)
					{
						matrix.Append("|");
					}
				}
				else
				{
					matrix.Append(emptyLine);
				}

				matrix.AppendLine();
			}
			return matrix.ToString();
		}*/

		private static void findContradictionsInCell(HashSet<IConcept> signs, IConcept left, IConcept right, List<Contradiction> foundContradictions)
		{
			if (doesOneOrMoreContradictedSignsPairExist(signs) || doesValueContradictToItself(signs, left, right))
			{
				if (!foundContradictions.Any(c => c.Value1 == right && c.Value2 == left))
				{
					foundContradictions.Add(new Contradiction(left, right, signs));
				}
			}
		}

		private static Boolean doesOneOrMoreContradictedSignsPairExist(ICollection<IConcept> signs)
		{
			foreach (var sign1 in signs)
			{
				foreach (var sign2 in signs)
				{
					if (ComparisonStatement.Contradictions.Any(tuple =>
						(tuple.Item1 == sign1 && tuple.Item2 == sign2) ||
						(tuple.Item1 == sign2 && tuple.Item2 == sign1)))
					{
						return true;
					}
				}
			}
			return false;
		}

		private static Boolean doesValueContradictToItself(HashSet<IConcept> signs, IConcept left, IConcept right)
		{
			return left == right && signs.Any(s => s != SystemConcepts.IsEqualTo);
		}
	}

	public class ComparisonStatementContradictionsChecker : ContradictionsChecker<ComparisonStatement>
	{
		public ComparisonStatementContradictionsChecker(IEnumerable<ComparisonStatement> statements)
			: base(statements)
		{ }

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
	}
}
