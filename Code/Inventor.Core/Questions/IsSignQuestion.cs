using System;
using System.Collections.Generic;

using Inventor.Core.Statements;

namespace Inventor.Core.Questions
{
	public class IsSignQuestion : Question, IQuestion<HasSignStatement>
	{
		public IConcept Concept
		{ get; }

		public IsSignQuestion(IConcept concept, IEnumerable<IStatement> preconditions = null)
			: base(preconditions)
		{
			if (concept == null) throw new ArgumentNullException(nameof(concept));

			Concept = concept;
		}
	}
}
