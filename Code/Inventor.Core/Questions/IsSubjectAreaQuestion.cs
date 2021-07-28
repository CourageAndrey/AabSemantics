using System;

namespace Inventor.Core.Questions
{
	public sealed class IsSubjectAreaQuestion : Question
	{
		public IConcept Concept
		{ get; }

		public IConcept Area
		{ get; }

		public IsSubjectAreaQuestion(IConcept concept, IConcept area)
		{
			if (concept == null) throw new ArgumentNullException(nameof(concept));
			if (area == null) throw new ArgumentNullException(nameof(area));

			Concept = concept;
			Area = area;
		}
	}
}
