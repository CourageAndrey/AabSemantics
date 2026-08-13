using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AabSemantics.Utils;

namespace AabSemantics.Statements
{
	/// <summary>
	/// Finds contradictions among comparison-like statements. It builds a matrix of the known
	/// relation signs between every pair of values, repeatedly infers new cells from existing
	/// ones until nothing changes, and then reports the pairs that ended up with mutually
	/// exclusive signs.
	/// </summary>
	/// <typeparam name="StatementT">Statement type expressing a relation between two values.</typeparam>
	public abstract class ContradictionsChecker<StatementT>
		where StatementT : IStatement
	{
		/// <summary>Every value mentioned by the statements.</summary>
		protected readonly HashSet<IConcept> AllValues; // all unique involved values

		/// <summary>Signs known for each ordered pair of values, indexed row then column.</summary>
		protected readonly Dictionary<IConcept, Dictionary<IConcept, HashSet<IConcept>>> AllSigns; // matrix of known signs

		/// <summary>Seeds the matrix from the given statements.</summary>
		/// <param name="statements">Statements to analyse.</param>
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

		/// <summary>Reads the left-hand value of a statement.</summary>
		/// <param name="statement">Statement to read.</param>
		/// <returns>The left value.</returns>
		protected abstract IConcept GetLeftValue(StatementT statement);

		/// <summary>Reads the right-hand value of a statement.</summary>
		/// <param name="statement">Statement to read.</param>
		/// <returns>The right value.</returns>
		protected abstract IConcept GetRightValue(StatementT statement);

		/// <summary>Reads the relation sign of a statement.</summary>
		/// <param name="statement">Statement to read.</param>
		/// <returns>The sign.</returns>
		protected abstract IConcept GetSign(StatementT statement);

		/// <summary>Records a sign for a pair, plus whatever the sign's own semantics imply.</summary>
		/// <param name="valueRow">Row value.</param>
		/// <param name="valueColumn">Column value.</param>
		/// <param name="sign">Sign to record.</param>
		/// <returns><c>true</c> if the matrix changed.</returns>
		protected abstract Boolean SetCombinationWithDescendants(IConcept valueRow, IConcept valueColumn, IConcept sign);

		/// <summary>Decides whether a set of signs recorded for one pair is self-contradictory.</summary>
		/// <param name="signs">Signs recorded for the pair.</param>
		/// <param name="left">Left value of the pair.</param>
		/// <param name="right">Right value of the pair.</param>
		/// <returns><c>true</c> if the signs cannot hold together.</returns>
		protected abstract Task<Boolean> ContradictsAsync(HashSet<IConcept> signs, IConcept left, IConcept right);

		/// <summary>Infers the sign between two values from the signs linking each of them to a third.</summary>
		/// <param name="valueRow">First value.</param>
		/// <param name="signRow">Sign between the first value and the intermediate one.</param>
		/// <param name="signColumn">Sign between the intermediate value and the second one.</param>
		/// <param name="valueColumn">Second value.</param>
		/// <returns><c>true</c> if the matrix changed.</returns>
		protected abstract Boolean TryToUpdateCombinations(IConcept valueRow, IConcept signRow, IConcept signColumn, IConcept valueColumn);

		/// <summary>Infers everything derivable, then reports the contradictions found.</summary>
		/// <returns>One entry per contradicting pair of values; empty when the statements are consistent.</returns>
		public async Task<List<Contradiction>> CheckForContradictionsAsync()
		{
			while (await UpdateInferredCombinationsAsync())
			{ }

			return await FindContradictionsInMatrixAsync();
		}

		private async Task<Boolean> UpdateInferredCombinationsAsync()
		{
			Boolean combinationsUpdated = false;
			await Task.Run(async () =>
			{
				foreach (var row in AllValues)
				{
					foreach (var column in AllValues)
					{
						if (row != column)
						{
							combinationsUpdated |= await UpdateInferredCombinationsFromCellAsync(row, column).ConfigureAwait(false);
						}
					}
				}
			}).ConfigureAwait(false);
			return combinationsUpdated;
		}

		private async Task<Boolean> UpdateInferredCombinationsFromCellAsync(IConcept row, IConcept column)
		{
			Dictionary<IConcept, HashSet<IConcept>> combinationsRow;
			HashSet<IConcept> signsRow;
			Dictionary<IConcept, HashSet<IConcept>> combinationsColumn;

			return	AllSigns.TryGetValue(row, out combinationsRow) &&
					combinationsRow.TryGetValue(column, out signsRow) && // if value in current cell is set
					AllSigns.TryGetValue(column, out combinationsColumn) && // if current value has comparisons with other values
					await UpdateAllInferredCombinationsWithinCellAsync(row, combinationsColumn, signsRow);
		}

		private async Task<Boolean> UpdateAllInferredCombinationsWithinCellAsync(
			IConcept valueRow,
			Dictionary<IConcept, HashSet<IConcept>> combinationsColumn,
			HashSet<IConcept> signsRow)
		{
			Boolean combinationsUpdated = false;
			foreach (var kvp in combinationsColumn)
			{
				var valueColumn = kvp.Key;
				var signsColumn = kvp.Value;

				foreach (var signRow in await signsRow.ToListAsync())
				{
					foreach (var signColumn in await signsColumn.ToListAsync())
					{
						combinationsUpdated |= TryToUpdateCombinations(valueRow, signRow, signColumn, valueColumn);
					}
				}
			}
			return combinationsUpdated;
		}

		private async Task<List<Contradiction>> FindContradictionsInMatrixAsync()
		{
			var foundContradictions = new List<Contradiction>();

			foreach (var leftCombinations in AllSigns)
			{
				var left = leftCombinations.Key;
				foreach (var rightCombinations in leftCombinations.Value)
				{
					var right = rightCombinations.Key;
					var signs = rightCombinations.Value;
					await FindContradictionsInCellAsync(signs, left, right, foundContradictions);
				}
			}

			return foundContradictions;
		}

		/// <summary>Records a single sign for a pair, creating the matrix row and cell as needed.</summary>
		/// <param name="left">Row value.</param>
		/// <param name="right">Column value.</param>
		/// <param name="sign">Sign to record.</param>
		/// <returns><c>true</c> if the matrix changed; <c>false</c> when the sign was already recorded.</returns>
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

		/*protected String Display()
		{
			var align = new Func<String, int, String>((text, lenght) => text.PadLeft(lenght, ' '));

			var signSymbols = new Dictionary<IConcept, String>
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
			String columnsHeader = String.Join("|", headers.Values.Select(h => align(h, columnWidth)));
			String tableHeader = new String(' ', headersMaxLength) + "|" + columnsHeader;
			String afterHeaderLine = new String(tableHeader.Select(c => c == '|' ? '+' : '-').ToArray());
			String emptyLine = new String(columnsHeader.Select(c => c == '|' ? '|' : ' ').ToArray());
			String emptyCell = new String(' ', columnWidth);

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
							matrix.Append(align(String.Join(String.Empty, cellValue.Select(s => signSymbols[s])), columnWidth));
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

		private async Task FindContradictionsInCellAsync(HashSet<IConcept> signs, IConcept left, IConcept right, List<Contradiction> foundContradictions)
		{
			if (await ContradictsAsync(signs, left, right))
			{
				if (! await foundContradictions.AnyAsync(c => c.Value1 == right && c.Value2 == left))
				{
					foundContradictions.Add(new Contradiction(left, right, signs));
				}
			}
		}
	}
}
