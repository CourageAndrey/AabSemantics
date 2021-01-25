using System;

namespace Inventor.Core.Questions
{
	public sealed class EnumerateContainersQuestion : IQuestion
	{
		public IConcept Concept
		{ get; }

		public EnumerateContainersQuestion(IConcept concept)
		{
			if (concept == null) throw new ArgumentNullException(nameof(concept));

			Concept = concept;
		}
	}
}
