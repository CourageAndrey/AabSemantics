using System;

namespace Inventor.Core.Questions
{
	public sealed class EnumeratePartsQuestion : Question
	{
		public IConcept Concept
		{ get; }

		public EnumeratePartsQuestion(IConcept concept)
		{
			if (concept == null) throw new ArgumentNullException(nameof(concept));

			Concept = concept;
		}
	}
}
