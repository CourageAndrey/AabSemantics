using System;
using System.Collections.Generic;

namespace Inventor.Core.Questions
{
	public class IsSignQuestion : Question
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
