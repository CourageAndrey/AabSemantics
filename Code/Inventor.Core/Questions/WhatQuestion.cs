using System;
using System.Collections.Generic;

namespace Inventor.Core.Questions
{
	public sealed class WhatQuestion : Question
	{
		public IConcept Concept
		{ get; }

		public WhatQuestion(IConcept concept, IEnumerable<IStatement> preconditions = null)
			: base(preconditions)
		{
			if (concept == null) throw new ArgumentNullException(nameof(concept));

			Concept = concept;
		}
	}
}
