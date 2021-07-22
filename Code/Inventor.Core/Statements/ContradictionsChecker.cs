using System;
using System.Collections.Generic;
using System.Linq;

namespace Inventor.Core.Statements
{
	public abstract class ContradictionsChecker<StatementT>
		where StatementT : IStatement
	{
		protected readonly HashSet<IConcept> AllValues; // all unique involved values
		protected readonly Dictionary<IConcept, Dictionary<IConcept, HashSet<IConcept>>> AllSigns; // matrix of known signs

		protected ContradictionsChecker(IEnumerable<StatementT> statements)
		{
			AllValues = new HashSet<IConcept>();
			AllSigns = new Dictionary<IConcept, Dictionary<IConcept, HashSet<IConcept>>>();

			foreach (var statement in statements)
			{
				var leftValue = GetLeftValue(statement);
				var rightValue = GetRightValue(statement);
				var sign = GetSign(statement);

				AllValues.Add(leftValue);
				AllValues.Add(rightValue);

				SetCombinationWithDescendants(leftValue, rightValue, sign);
			}
		}

		protected abstract IConcept GetLeftValue(StatementT statement);

		protected abstract IConcept GetRightValue(StatementT statement);

		protected abstract IConcept GetSign(StatementT statement);

		protected abstract Boolean SetCombinationWithDescendants(IConcept valueRow, IConcept valueColumn, IConcept sign);

		protected abstract Boolean Contradicts(HashSet<IConcept> signs, IConcept left, IConcept right);

		protected abstract Boolean TryToUpdateCombinations(IConcept valueRow, IConcept signRow, IConcept signColumn, IConcept valueColumn);

		public List<Contradiction> CheckForContradictions()
		{
			while (updateInferredCombinations())
			{ }

			return findContradictionsInMatrix();
		}

		private Boolean updateInferredCombinations()
		{
			Boolean combinationsUpdated = false;
			foreach (var row in AllValues)
			{
				foreach (var column in AllValues)
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
			if (AllSigns.TryGetValue(row, out combinationsRow) && combinationsRow.TryGetValue(column, out signsRow)) // if value in current cell is set
			{
				Dictionary<IConcept, HashSet<IConcept>> combinationsColumn;
				if (AllSigns.TryGetValue(column, out combinationsColumn)) // if current value has comparisons with other values
				{
					return updateAllInferredCombinationsWithinCell(row, combinationsColumn, signsRow);
				}
			}
			return false;
		}

		private Boolean updateAllInferredCombinationsWithinCell(
			IConcept valueRow,
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
						combinationsUpdated |= TryToUpdateCombinations(valueRow, signRow, signColumn, valueColumn);
					}
				}
			}
			return combinationsUpdated;
		}

		private List<Contradiction> findContradictionsInMatrix()
		{
			var foundContradictions = new List<Contradiction>();
			foreach (var leftCombinations in AllSigns)
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

		protected Boolean SetCombination(IConcept left, IConcept right, IConcept sign)
		{
			Boolean updated = false;

			// get "row" using LEFT value as key, return true if row is added
			Dictionary<IConcept, HashSet<IConcept>> combinations;
			if (!AllSigns.TryGetValue(left, out combinations))
			{
				AllSigns[left] = combinations = new Dictionary<IConcept, HashSet<IConcept>>();
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

		/*protected string Display()
		{
			var align = new Func<string, int, string>((text, lenght) => text.PadLeft(lenght, ' '));

			var signSymbols = new Dictionary<IConcept, string>
			{
				{ ComparisonSigns.IsEqualTo, "=" },
				{ ComparisonSigns.IsNotEqualTo, "≠" },
				{ ComparisonSigns.IsGreaterThanOrEqualTo, "≥" },
				{ ComparisonSigns.IsGreaterThan, ">" },
				{ ComparisonSigns.IsLessThanOrEqualTo, "≤" },
				{ ComparisonSigns.IsLessThan, "<" },
				{ SequenceSigns.StartsAfterOtherStarted, "SAS," },
				{ SequenceSigns.StartsWhenOtherStarted, "SWS," },
				{ SequenceSigns.StartsBeforeOtherStarted, "SBS," },
				{ SequenceSigns.FinishesAfterOtherStarted, "FAS," },
				{ SequenceSigns.FinishesWhenOtherStarted, "FWS," },
				{ SequenceSigns.FinishesBeforeOtherStarted, "FBS," },
				{ SequenceSigns.StartsAfterOtherFinished, "SAF," },
				{ SequenceSigns.StartsWhenOtherFinished, "SWF," },
				{ SequenceSigns.StartsBeforeOtherFinished, "SBF," },
				{ SequenceSigns.FinishesAfterOtherFinished, "FAF," },
				{ SequenceSigns.FinishesWhenOtherFinished, "FWF," },
				{ SequenceSigns.FinishesBeforeOtherFinished, "FBF," },
				{ SequenceSigns.Causes, "CCC," },
				{ SequenceSigns.IsCausedBy, "ICB," },
				{ SequenceSigns.SimultaneousWith, "SWW," },
			};

			var headers = AllValues.ToDictionary(
				value => value,
				value => value.Name.GetValue(Localization.Language.Default));
			int headersMaxLength = headers.Values.Max(h => h.Length);

			int signsMaxCount = int.MinValue;
			foreach (var dictionary in AllSigns.Values)
			{
				signsMaxCount = Math.Max(signsMaxCount, dictionary.Values.Max(list => list.Count));
			}

			int columnWidth = Math.Max(headersMaxLength, signsMaxCount * signSymbols.Values.Max(v => v.Length));
			string columnsHeader = string.Join("|", headers.Values.Select(h => align(h, columnWidth)));
			string tableHeader = new string(' ', headersMaxLength) + "|" + columnsHeader;
			string afterHeaderLine = new string(tableHeader.Select(c => c == '|' ? '+' : '-').ToArray());
			string emptyLine = new string(columnsHeader.Select(c => c == '|' ? '|' : ' ').ToArray());
			string emptyCell = new string(' ', columnWidth);

			var matrix = new System.Text.StringBuilder();
			matrix.AppendLine(tableHeader);
			matrix.AppendLine(afterHeaderLine);

			foreach (var value1 in AllValues)
			{
				matrix.Append(align(headers[value1], headersMaxLength));
				matrix.Append("|");

				Dictionary<IConcept, HashSet<IConcept>> row;
				if (AllSigns.TryGetValue(value1, out row))
				{
					foreach (var value2 in AllValues)
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

						if (AllValues.Last() != value2)
						{
							matrix.Append("|");
						}
					}

					if (AllValues.Last() != value1)
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

		private void findContradictionsInCell(HashSet<IConcept> signs, IConcept left, IConcept right, List<Contradiction> foundContradictions)
		{
			if (Contradicts(signs, left, right))
			{
				if (!foundContradictions.Any(c => c.Value1 == right && c.Value2 == left))
				{
					foundContradictions.Add(new Contradiction(left, right, signs));
				}
			}
		}
	}
}
