using System;
using System.Collections.Generic;

namespace Inventor.Core.Questions
{
	public sealed class DescribeSubjectAreaQuestion : Question
	{
		public IConcept Concept
		{ get; }

		public DescribeSubjectAreaQuestion(IConcept concept, IEnumerable<IStatement> preconditions = null)
			: base(preconditions)
		{
			if (concept == null) throw new ArgumentNullException(nameof(concept));

			Concept = concept;
		}
	}
}
