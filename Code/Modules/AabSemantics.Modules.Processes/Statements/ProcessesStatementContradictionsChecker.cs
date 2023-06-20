using System.Collections.Generic;

using AabSemantics.Modules.Processes.Concepts;
using AabSemantics.Statements;

namespace AabSemantics.Modules.Processes.Statements
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

		protected override System.Boolean Contradicts(HashSet<IConcept> signs, IConcept left, IConcept right)
		{
			return signs.Contradicts();
		}

		protected override System.Boolean TryToUpdateCombinations(IConcept valueRow, IConcept signRow, IConcept signColumn, IConcept valueColumn)
		{
			var resultSign = SequenceSigns.TryToCombineMutualSequences(signRow, signColumn);
			return resultSign != null && SetCombinationWithDescendants(valueRow, valueColumn, resultSign);
		}
	}
}