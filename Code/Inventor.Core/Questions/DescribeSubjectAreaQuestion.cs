using System;

namespace Inventor.Core.Questions
{
	public sealed class DescribeSubjectAreaQuestion : IQuestion
	{
		public IConcept Concept
		{ get; }

		public DescribeSubjectAreaQuestion(IConcept concept)
		{
			if (concept == null) throw new ArgumentNullException(nameof(concept));

			Concept = concept;
		}
	}
}
