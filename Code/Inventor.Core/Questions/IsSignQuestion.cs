using System;

namespace Inventor.Core.Questions
{
	public class IsSignQuestion : IQuestion
	{
		public IConcept Concept
		{ get; }

		public IsSignQuestion(IConcept concept)
		{
			if (concept == null) throw new ArgumentNullException(nameof(concept));

			Concept = concept;
		}
	}
}
