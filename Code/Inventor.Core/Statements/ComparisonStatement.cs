using System;
using System.Collections.Generic;
using System.Linq;

using Inventor.Core.Attributes;
using Inventor.Core.Base;
using Inventor.Core.Localization;
using Inventor.Core.Questions;

namespace Inventor.Core.Statements
{
	public class ComparisonStatement : Statement<ComparisonStatement>
	{
		#region Properties

		public IConcept LeftValue
		{ get; private set; }

		public IConcept RightValue
		{ get; private set; }

		public IConcept ComparisonSign
		{ get; private set; }

		#endregion

		public ComparisonStatement(IConcept leftValue, IConcept rightValue, IConcept comparisonSign)
			: base(new Func<ILanguage, String>(language => language.StatementNames.Comparison), new Func<ILanguage, String>(language => language.StatementHints.Comparison))
		{
			Update(leftValue, rightValue, comparisonSign);
		}

		public void Update(IConcept leftValue, IConcept rightValue, IConcept comparisonSign)
		{
			if (leftValue == null) throw new ArgumentNullException(nameof(leftValue));
			if (rightValue == null) throw new ArgumentNullException(nameof(rightValue));
			if (comparisonSign == null) throw new ArgumentNullException(nameof(comparisonSign));
			if (!leftValue.HasAttribute<IsValueAttribute>()) throw new ArgumentException("Left value concept has to be marked as IsValue Attribute.", nameof(leftValue));
			if (!rightValue.HasAttribute<IsValueAttribute>()) throw new ArgumentException("Right value concept has to be marked as IsValue Attribute.", nameof(rightValue));
			if (!comparisonSign.HasAttribute<IsComparisonSignAttribute>()) throw new ArgumentException("Comparison Sign concept has to be marked as IsComparisonSign Attribute.", nameof(comparisonSign));

			LeftValue = leftValue;
			RightValue = rightValue;
			ComparisonSign = comparisonSign;
		}

		public override IEnumerable<IConcept> GetChildConcepts()
		{
			yield return LeftValue;
			yield return RightValue;
			yield return ComparisonSign;
		}

		#region Description

		protected override IDictionary<String, INamed> GetDescriptionParameters()
		{
			return new Dictionary<String, INamed>
			{
				{ Strings.ParamLeftValue, LeftValue },
				{ Strings.ParamRightValue, RightValue },
				{ Strings.ParamComparisonSign, ComparisonSign },
			};
		}

		protected override Func<String> GetDescriptionText(ILanguageStatements language)
		{
			return () => language.Comparison;
		}

		#endregion

		#region Consistency checking

		public override Boolean Equals(ComparisonStatement other)
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

		internal static readonly ICollection<Tuple<IConcept, IConcept>> Contradictions = new List<Tuple<IConcept, IConcept>>
		{
			new Tuple<IConcept, IConcept>(SystemConcepts.IsEqualTo, SystemConcepts.IsNotEqualTo),
			new Tuple<IConcept, IConcept>(SystemConcepts.IsEqualTo, SystemConcepts.IsGreaterThan),
			new Tuple<IConcept, IConcept>(SystemConcepts.IsEqualTo, SystemConcepts.IsLessThan),
			new Tuple<IConcept, IConcept>(SystemConcepts.IsGreaterThan, SystemConcepts.IsLessThan),
			new Tuple<IConcept, IConcept>(SystemConcepts.IsGreaterThan, SystemConcepts.IsLessThanOrEqualTo),
			new Tuple<IConcept, IConcept>(SystemConcepts.IsLessThan, SystemConcepts.IsGreaterThanOrEqualTo),
		};

		#endregion

		public ComparisonStatement SwapOperandsToMatchOrder(ComparisonQuestion question)
		{
			return RightValue == question.LeftValue || LeftValue == question.RightValue
				? SwapOperands()
				: this;
		}

		public ComparisonStatement SwapOperands()
		{
			return new ComparisonStatement(leftValue: RightValue, rightValue: LeftValue, ComparisonSign.Revert());
		}
	}

	public static class ComparisonStatementConsistency
	{
		public static List<Contradiction> CheckForContradictions(this IEnumerable<ComparisonStatement> statements)
		{
			HashSet<IConcept> allValues; // all unique involved values
			Dictionary<IConcept, Dictionary<IConcept, HashSet<IConcept>>> allSigns; // matrix of known signs
			initializeValueMatrix(statements, out allValues, out allSigns);

			makeAllValuesAlwaysEqualToThemselves(allValues, allSigns);

			while (updateInferredCombinations(allValues, allSigns))
			{ }

			return findContradictionsInMatrix(allSigns);
		}

		private static void initializeValueMatrix(
			IEnumerable<ComparisonStatement> statements,
			out HashSet<IConcept> allValues,
			out Dictionary<IConcept, Dictionary<IConcept, HashSet<IConcept>>> allSigns)
		{
			allValues = new HashSet<IConcept>();
			allSigns = new Dictionary<IConcept, Dictionary<IConcept, HashSet<IConcept>>>();

			foreach (var comparison in statements)
			{
				allValues.Add(comparison.LeftValue);
				allValues.Add(comparison.RightValue);

				setCombination(allSigns, comparison.LeftValue, comparison.RightValue, comparison.ComparisonSign);
				if (canBeReverted(comparison.ComparisonSign))
				{
					setCombination(allSigns, comparison.RightValue, comparison.LeftValue, comparison.ComparisonSign.Revert());
				}
			}
		}

		private static Boolean canBeReverted(IConcept sign)
		{
			return sign != SystemConcepts.IsEqualTo && sign != SystemConcepts.IsNotEqualTo;
		}

		private static void makeAllValuesAlwaysEqualToThemselves(HashSet<IConcept> allValues, Dictionary<IConcept, Dictionary<IConcept, HashSet<IConcept>>> allSigns)
		{
			foreach (var value in allValues)
			{
				setCombination(allSigns, value, value, SystemConcepts.IsEqualTo);
			}
		}

		private static Boolean updateInferredCombinations(HashSet<IConcept> allValues, Dictionary<IConcept, Dictionary<IConcept, HashSet<IConcept>>> allSigns)
		{
			Boolean combinationsUpdated = false;
			foreach (var row in allValues)
			{
				foreach (var column in allValues)
				{
					if (row != column)
					{
						combinationsUpdated |= updateInferredCombinationsFromCell(allSigns, row, column);
					}
				}
			}
			return combinationsUpdated;
		}

		private static Boolean updateInferredCombinationsFromCell(Dictionary<IConcept, Dictionary<IConcept, HashSet<IConcept>>> allSigns, IConcept row, IConcept column)
		{
			Dictionary<IConcept, HashSet<IConcept>> combinationsRow;
			HashSet<IConcept> signsRow;
			if (allSigns.TryGetValue(row, out combinationsRow) && combinationsRow.TryGetValue(column, out signsRow)) // if value in current cell is set
			{
				Dictionary<IConcept, HashSet<IConcept>> combinationsColumn;
				if (allSigns.TryGetValue(column, out combinationsColumn)) // if current value has comparisons with other values
				{
					return updateAllInferredCombinationsWithinCell(allSigns, row, combinationsColumn, signsRow);
				}
			}
			return false;
		}

		private static Boolean updateAllInferredCombinationsWithinCell(
			Dictionary<IConcept, Dictionary<IConcept, HashSet<IConcept>>> allSigns,
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
						combinationsUpdated |= tryToUpdateCombinations(allSigns, row, signRow, signColumn, valueColumn);
					}
				}
			}
			return combinationsUpdated;
		}

		private static Boolean tryToUpdateCombinations(
			Dictionary<IConcept, Dictionary<IConcept, HashSet<IConcept>>> allSigns,
			IConcept row,
			IConcept signRow, 
			IConcept signColumn,
			IConcept valueColumn)
		{
			Boolean combinationsUpdated = false;

			var resultSign = SystemConcepts.CompareThreeValues(signRow, signColumn);
			if (resultSign != null)
			{
				combinationsUpdated |= setCombination(allSigns, row, valueColumn, resultSign);
				if (canBeReverted(resultSign))
				{
					combinationsUpdated |= setCombination(allSigns, valueColumn, row, resultSign.Revert());
				}
			}

			return combinationsUpdated;
		}

		private static List<Contradiction> findContradictionsInMatrix(Dictionary<IConcept, Dictionary<IConcept, HashSet<IConcept>>> allSigns)
		{
			var foundContradictions = new List<Contradiction>();
			foreach (var leftCombinations in allSigns)
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

		private static Boolean setCombination(Dictionary<IConcept, Dictionary<IConcept, HashSet<IConcept>>> allSigns, IConcept left, IConcept right, IConcept sign)
		{
			Boolean updated = false;

			// get "row" using LEFT value as key, return true if row is added
			Dictionary<IConcept, HashSet<IConcept>> combinations;
			if (!allSigns.TryGetValue(left, out combinations))
			{
				allSigns[left] = combinations = new Dictionary<IConcept, HashSet<IConcept>>();
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

		/*private static string display(
			ICollection<IConcept> allValues,
			IDictionary<IConcept, Dictionary<IConcept, HashSet<IConcept>>> allSigns)
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

			var headers = allValues.ToDictionary(
				value => value,
				value => value.Name.GetValue(Language.Default));
			int headersMaxLength = headers.Values.Max(h => h.Length);

			int signsMaxCount = int.MinValue;
			foreach (var dictionary in allSigns.Values)
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

			foreach (var value1 in allValues)
			{
				matrix.Append(align(headers[value1], headersMaxLength));
				matrix.Append("|");

				Dictionary<IConcept, HashSet<IConcept>> row;
				if (allSigns.TryGetValue(value1, out row))
				{
					foreach (var value2 in allValues)
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

						if (allValues.Last() != value2)
						{
							matrix.Append("|");
						}
					}

					if (allValues.Last() != value1)
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
}
