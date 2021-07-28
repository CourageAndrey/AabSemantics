using System;
using System.Collections.Generic;

namespace Inventor.Core.Questions
{
	public sealed class EnumerateChildrenQuestion : Question
	{
		public IConcept Concept
		{ get; }

		public EnumerateChildrenQuestion(IConcept concept, IEnumerable<IStatement> preconditions = null)
			: base(preconditions)
		{
			if (concept == null) throw new ArgumentNullException(nameof(concept));

			Concept = concept;
		}
	}
}
