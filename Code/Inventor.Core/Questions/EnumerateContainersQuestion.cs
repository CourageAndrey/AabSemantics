using System;

namespace Inventor.Core.Questions
{
	public sealed class EnumerateContainersQuestion : Question
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
