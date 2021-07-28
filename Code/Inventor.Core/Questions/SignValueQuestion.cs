using System;

namespace Inventor.Core.Questions
{
	public sealed class SignValueQuestion : Question
	{
		public IConcept Concept
		{ get; }

		public IConcept Sign
		{ get; }

		public SignValueQuestion(IConcept concept, IConcept sign)
		{
			if (concept == null) throw new ArgumentNullException(nameof(concept));
			if (sign == null) throw new ArgumentNullException(nameof(sign));

			Concept = concept;
			Sign = sign;
		}
	}
}
