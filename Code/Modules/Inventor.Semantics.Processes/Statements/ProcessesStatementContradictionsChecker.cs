using System;
using System.Collections.Generic;

using Inventor.Semantics;
using Inventor.Semantics.Statements;
using Inventor.Semantics.Processes.Concepts;

namespace Inventor.Semantics.Processes.Statements
{
	public class ProcessesStatementContradictionsChecker : ContradictionsChecker<ProcessesStatement>
	{
		public ProcessesStatementContradictionsChecker(IEnumerable<ProcessesStatement> statements)
			: base(statements)
		{ }

		protected override IConcept GetLeftValue(ProcessesStatement statement)
		{
			return statement.ProcessA;
		}

		protected override IConcept GetRightValue(ProcessesStatement statement)
		{
			return statement.ProcessB;
		}

		protected override IConcept GetSign(ProcessesStatement statement)
		{
			return statement.SequenceSign;
		}

		protected override Boolean SetCombinationWithDescendants(IConcept valueRow, IConcept valueColumn, IConcept sign)
		{
			Boolean combinationsUpdated = SetCombination(valueRow, valueColumn, sign);
			combinationsUpdated |= SetCombination(valueColumn, valueRow, SequenceSigns.Revert(sign));
			foreach (var consequentSign in sign.Consequently())
			{
				combinationsUpdated |= SetCombination(valueRow, valueColumn, consequentSign);
			}
			return combinationsUpdated;
		}

		protected override Boolean Contradicts(HashSet<IConcept> signs, IConcept left, IConcept right)
		{
			return signs.Contradicts();
		}

		protected override Boolean TryToUpdateCombinations(IConcept valueRow, IConcept signRow, IConcept signColumn, IConcept valueColumn)
		{
			var resultSign = SequenceSigns.TryToCombineMutualSequences(signRow, signColumn);
			return resultSign != null && SetCombinationWithDescendants(valueRow, valueColumn, resultSign);
		}
	}
}