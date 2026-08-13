using System.Collections.Generic;
using System.Threading.Tasks;

using AabSemantics.Modules.Processes.Concepts;
using AabSemantics.Statements;

namespace AabSemantics.Modules.Processes.Statements
{
	/// <summary>
	/// Finds contradictions among process sequence statements. It expands each recorded sign into
	/// its consequences and its reverted form, then composes transitive signs across shared
	/// processes until nothing new follows.
	/// </summary>
	public class ProcessesStatementContradictionsChecker : ContradictionsChecker<ProcessesStatement>
	{
		/// <summary>Seeds the matrix from the given statements.</summary>
		/// <param name="statements">Statements to analyse.</param>
		public ProcessesStatementContradictionsChecker(IEnumerable<ProcessesStatement> statements)
			: base(statements)
		{ }

		/// <summary>Reads the statement's first process.</summary>
		/// <param name="statement">Statement to read.</param>
		/// <returns>The first process.</returns>
		protected override IConcept GetLeftValue(ProcessesStatement statement)
		{
			return statement.ProcessA;
		}

		/// <summary>Reads the statement's second process.</summary>
		/// <param name="statement">Statement to read.</param>
		/// <returns>The second process.</returns>
		protected override IConcept GetRightValue(ProcessesStatement statement)
		{
			return statement.ProcessB;
		}

		/// <summary>Reads the statement's sequence sign.</summary>
		/// <param name="statement">Statement to read.</param>
		/// <returns>The sign.</returns>
		protected override IConcept GetSign(ProcessesStatement statement)
		{
			return statement.SequenceSign;
		}

		/// <summary>Records a sign together with everything it implies, and the reverted sign in the transposed cell.</summary>
		/// <param name="valueRow">First process.</param>
		/// <param name="valueColumn">Second process.</param>
		/// <param name="sign">Sign to record.</param>
		/// <returns><c>true</c> if the matrix changed.</returns>
		protected override System.Boolean SetCombinationWithDescendants(IConcept valueRow, IConcept valueColumn, IConcept sign)
		{
			System.Boolean combinationsUpdated = SetCombination(valueRow, valueColumn, sign);
			combinationsUpdated |= SetCombination(valueColumn, valueRow, SequenceSigns.Revert(sign));
			foreach (var consequentSign in sign.Consequently())
			{
				combinationsUpdated |= SetCombination(valueRow, valueColumn, consequentSign);
			}
			return combinationsUpdated;
		}

		/// <summary>Reports a contradiction when the recorded signs cannot hold together for the pair.</summary>
		/// <param name="signs">Signs recorded for the pair.</param>
		/// <param name="left">First process.</param>
		/// <param name="right">Second process.</param>
		/// <returns><c>true</c> if the signs conflict.</returns>
		protected override async Task<System.Boolean> ContradictsAsync(HashSet<IConcept> signs, IConcept left, IConcept right)
		{
			return await signs.ContradictsAsync();
		}

		/// <summary>Derives the sign between two processes from their signs against a shared middle process.</summary>
		/// <param name="valueRow">First process.</param>
		/// <param name="signRow">Sign between it and the middle process.</param>
		/// <param name="signColumn">Sign between the middle process and the second one.</param>
		/// <param name="valueColumn">Second process.</param>
		/// <returns><c>true</c> if a sign followed and the matrix changed.</returns>
		protected override System.Boolean TryToUpdateCombinations(IConcept valueRow, IConcept signRow, IConcept signColumn, IConcept valueColumn)
		{
			var resultSign = SequenceSigns.TryToCombineMutualSequences(signRow, signColumn);
			return resultSign != null && SetCombinationWithDescendants(valueRow, valueColumn, resultSign);
		}
	}
}