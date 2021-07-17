using System;
using System.Collections.Generic;
using System.Linq;

using Inventor.Core.Attributes;
using Inventor.Core.Base;
using Inventor.Core.Localization;
using Inventor.Core.Questions;

namespace Inventor.Core.Statements
{
	public class ProcessesStatement : Statement<ProcessesStatement>
	{
		#region Properties

		public IConcept ProcessA
		{ get; private set; }

		public IConcept ProcessB
		{ get; private set; }

		public IConcept SequenceSign
		{ get; private set; }

		#endregion

		public ProcessesStatement(IConcept processA, IConcept processB, IConcept sequenceSign)
			: base(new Func<ILanguage, String>(language => language.StatementNames.Processes), new Func<ILanguage, String>(language => language.StatementHints.Processes))
		{
			Update(processA, processB, sequenceSign);
		}

		public void Update(IConcept processA, IConcept processB, IConcept sequenceSign)
		{
			if (processA == null) throw new ArgumentNullException(nameof(processA));
			if (processB == null) throw new ArgumentNullException(nameof(processB));
			if (sequenceSign == null) throw new ArgumentNullException(nameof(sequenceSign));
			if (!processA.HasAttribute<IsProcessAttribute>()) throw new ArgumentException("Process A concept has to be marked as IsProcess Attribute.", nameof(processA));
			if (!processB.HasAttribute<IsProcessAttribute>()) throw new ArgumentException("Process B concept has to be marked as IsProcess Attribute.", nameof(processB));
			if (!sequenceSign.HasAttribute<IsSequenceSignAttribute>()) throw new ArgumentException("Sequence Sign concept has to be marked as IsSequenceSign Attribute.", nameof(sequenceSign));

			ProcessA = processA;
			ProcessB = processB;
			SequenceSign = sequenceSign;
		}

		public override IEnumerable<IConcept> GetChildConcepts()
		{
			yield return ProcessA;
			yield return ProcessB;
			yield return SequenceSign;
		}

		#region Description

		protected override Func<String> GetDescriptionText(ILanguageStatements language)
		{
			return () => language.Processes;
		}

		protected override IDictionary<String, INamed> GetDescriptionParameters()
		{
			return new Dictionary<String, INamed>
			{
				{ Strings.ParamProcessA, ProcessA },
				{ Strings.ParamProcessB, ProcessB },
				{ Strings.ParamSequenceSign, SequenceSign },
			};
		}

		#endregion

		#region Consistency checking

		public override Boolean Equals(ProcessesStatement other)
		{
			if (ReferenceEquals(this, other)) return true;
			if (other != null)
			{
				return	other.ProcessA == ProcessA &&
						other.ProcessB == ProcessB &&
						other.SequenceSign == SequenceSign;
			}
			else return false;
		}

		public static List<Contradiction> CheckForContradictions(IEnumerable<ProcessesStatement> statements)
		{
			var allProcesses = new HashSet<IConcept>();
			var allSigns = new Dictionary<IConcept, Dictionary<IConcept, HashSet<IConcept>>>();

			Func<IConcept, IConcept, IConcept, Boolean> setCombination = (left, right, sign) =>
			{
				Boolean updated = false;

				Dictionary<IConcept, HashSet<IConcept>> combinations;
				if (!allSigns.TryGetValue(left, out combinations))
				{
					allSigns[left] = combinations = new Dictionary<IConcept, HashSet<IConcept>>();
					updated = true;
				}

				HashSet<IConcept> signs;
				if (!combinations.TryGetValue(right, out signs))
				{
					combinations[right] = signs = new HashSet<IConcept>();
					updated = true;
				}

				Int32 countBefore = signs.Count;
				signs.Add(sign);
				if (signs.Count > countBefore)
				{
					updated = true;
				}

				return updated;
			};

			foreach (var comparison in statements)
			{
				allProcesses.Add(comparison.ProcessA);
				allProcesses.Add(comparison.ProcessB);
				setCombination(comparison.ProcessA, comparison.ProcessB, comparison.SequenceSign);
				setCombination(comparison.ProcessB, comparison.ProcessA, comparison.SequenceSign.Revert());
			}

			bool combinationsUpdated;
			do
			{
				combinationsUpdated = false;
				foreach (var row in allProcesses)
				{
					foreach (var column in allProcesses)
					{
						if (row != column)
						{
							Dictionary<IConcept, HashSet<IConcept>> combinationsRow;
							HashSet<IConcept> signsRow;
							if (allSigns.TryGetValue(row, out combinationsRow) && combinationsRow.TryGetValue(column, out signsRow))
							{
								Dictionary<IConcept, HashSet<IConcept>> combinationsColumn;
								if (allSigns.TryGetValue(row, out combinationsColumn))
								{
									foreach (var kvp in combinationsColumn)
									{
										var valueColumn = kvp.Key;
										var signsColumn = kvp.Value;

										foreach (var signRow in signsRow.ToList())
										{
											foreach (var signColumn in signsColumn.ToList())
											{
												var resultSign = SystemConcepts.CompareThreeValues(signRow, signColumn);
												if (resultSign != null)
												{
													combinationsUpdated |= setCombination(row, valueColumn, resultSign);
													combinationsUpdated |= setCombination(valueColumn, row, resultSign.Revert());
												}
											}
										}
									}
								}
							}
						}
					}
				}
			} while (combinationsUpdated);

			var foundContradictions = new List<Contradiction>();
			foreach (var leftCombinations in allSigns)
			{
				var left = leftCombinations.Key;
				foreach (var rightCombinations in leftCombinations.Value)
				{
					var right = rightCombinations.Key;
					var signs = rightCombinations.Value;
					if (contradicts(signs) || (left == right && signs.Any(s => s != SystemConcepts.IsEqualTo)))
					{
						if (!foundContradictions.Any(c => c.Value1 == right && c.Value2 == left))
						{
							foundContradictions.Add(new Contradiction(left, right, signs));
						}
					}
				}
			}
			return foundContradictions;
		}

		private static Boolean contradicts(ICollection<IConcept> signs)
		{
			if (signs.Contains(SystemConcepts.Causes) && signs.Contains(SystemConcepts.IsCausedBy))
			{
				return true;
			}

			if (signs.Contains(SystemConcepts.StartsBeforeOtherStarted) && signs.Contains(SystemConcepts.StartsAfterOtherFinished))
			{
				return true;
			}

			var foundStartSigns = signs.Where(s => startSigns.Contains(s)).ToList();
			var foundFinishSigns = signs.Where(s => finishSigns.Contains(s)).ToList();
			return foundStartSigns.Count > 1 || foundFinishSigns.Count > 1;
		}

		private static readonly ICollection<IConcept> startSigns = new HashSet<IConcept>
		{
			SystemConcepts.StartsAfterOtherStarted,
			SystemConcepts.StartsWhenOtherStarted,
			SystemConcepts.StartsBeforeOtherStarted,
			SystemConcepts.StartsAfterOtherFinished,
			SystemConcepts.StartsWhenOtherFinished,
			SystemConcepts.StartsBeforeOtherFinished,
		};

		private static readonly ICollection<IConcept> finishSigns = new HashSet<IConcept>
		{
			SystemConcepts.FinishesAfterOtherFinished,
			SystemConcepts.FinishesWhenOtherFinished,
			SystemConcepts.FinishesBeforeOtherFinished,
			SystemConcepts.FinishesAfterOtherStarted,
			SystemConcepts.FinishesWhenOtherStarted,
			SystemConcepts.FinishesBeforeOtherStarted,
		};

		#endregion

		public ProcessesStatement SwapOperandsToMatchOrder(ProcessesQuestion question)
		{
			return ProcessA == question.ProcessB || ProcessB == question.ProcessA
				? SwapOperands()
				: this;
		}

		public ProcessesStatement SwapOperands()
		{
			return new ProcessesStatement(processA: ProcessB, processB: ProcessA, SequenceSign.Revert());
		}
	}
}
