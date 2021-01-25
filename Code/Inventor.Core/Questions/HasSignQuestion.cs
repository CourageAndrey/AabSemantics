using System;

namespace Inventor.Core.Questions
{
	public sealed class HasSignQuestion : IQuestion
	{
		public IConcept Concept
		{ get; }

		public IConcept Sign
		{ get; }

		public Boolean Recursive
		{ get; }

		public HasSignQuestion(IConcept concept, IConcept sign, Boolean recursive)
		{
			if (concept == null) throw new ArgumentNullException(nameof(concept));
			if (sign == null) throw new ArgumentNullException(nameof(sign));

			Concept = concept;
			Sign = sign;
			Recursive = recursive;
		}
	}
}
