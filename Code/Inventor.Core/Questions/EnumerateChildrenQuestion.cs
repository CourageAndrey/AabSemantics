using System;

namespace Inventor.Core.Questions
{
	public sealed class EnumerateChildrenQuestion : IQuestion
	{
		public IConcept Concept
		{ get; }

		public EnumerateChildrenQuestion(IConcept concept)
		{
			if (concept == null) throw new ArgumentNullException(nameof(concept));

			Concept = concept;
		}
	}
}
