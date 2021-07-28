using System;

namespace Inventor.Core.Questions
{
	public sealed class ProcessesQuestion : Question
	{
		public IConcept ProcessA
		{ get; set; }

		public IConcept ProcessB
		{ get; set; }

		public ProcessesQuestion(IConcept processA, IConcept processB)
		{
			if (processA == null) throw new ArgumentNullException(nameof(processA));
			if (processB == null) throw new ArgumentNullException(nameof(processB));

			ProcessA = processA;
			ProcessB = processB;
		}
	}
}
