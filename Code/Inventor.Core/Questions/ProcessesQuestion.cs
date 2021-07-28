using System;
using System.Collections.Generic;

using Inventor.Core.Statements;

namespace Inventor.Core.Questions
{
	public sealed class ProcessesQuestion : Question<ProcessesQuestion>, IQuestion<ProcessesStatement>
	{
		#region Properties

		public IConcept ProcessA
		{ get; set; }

		public IConcept ProcessB
		{ get; set; }

		#endregion

		public ProcessesQuestion(IConcept processA, IConcept processB, IEnumerable<IStatement> preconditions = null)
			: base(preconditions)
		{
			if (processA == null) throw new ArgumentNullException(nameof(processA));
			if (processB == null) throw new ArgumentNullException(nameof(processB));

			ProcessA = processA;
			ProcessB = processB;
		}
	}
}
