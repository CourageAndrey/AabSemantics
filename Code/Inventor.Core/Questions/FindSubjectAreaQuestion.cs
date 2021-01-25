using System;

namespace Inventor.Core.Questions
{
	public sealed class FindSubjectAreaQuestion : IQuestion
	{
		public IConcept Concept
		{ get; }

		public FindSubjectAreaQuestion(IConcept concept)
		{
			if (concept == null) throw new ArgumentNullException(nameof(concept));

			Concept = concept;
		}
	}
}
