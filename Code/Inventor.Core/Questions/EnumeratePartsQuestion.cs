using System;
using System.Collections.Generic;

namespace Inventor.Core.Questions
{
	public sealed class EnumeratePartsQuestion : Question
	{
		public IConcept Concept
		{ get; }

		public EnumeratePartsQuestion(IConcept concept, IEnumerable<IStatement> preconditions = null)
			: base(preconditions)
		{
			if (concept == null) throw new ArgumentNullException(nameof(concept));

			Concept = concept;
		}
	}
}
