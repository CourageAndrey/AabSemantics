using System;
using System.Collections.Generic;

namespace Inventor.Core.Questions
{
	public sealed class EnumerateContainersQuestion : Question
	{
		public IConcept Concept
		{ get; }

		public EnumerateContainersQuestion(IConcept concept, IEnumerable<IStatement> preconditions = null)
			: base(preconditions)
		{
			if (concept == null) throw new ArgumentNullException(nameof(concept));

			Concept = concept;
		}
	}
}
