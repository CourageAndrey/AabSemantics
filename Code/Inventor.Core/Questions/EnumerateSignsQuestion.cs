using System;

namespace Inventor.Core.Questions
{
	public sealed class EnumerateSignsQuestion : IQuestion
	{
		public IConcept Concept
		{ get; }

		public Boolean Recursive
		{ get; }

		public EnumerateSignsQuestion(IConcept concept, Boolean recursive)
		{
			if (concept == null) throw new ArgumentNullException(nameof(concept));

			Concept = concept;
			Recursive = recursive;
		}
	}
}
