﻿using System;
using System.Collections.Generic;

namespace Inventor.Core.Questions
{
	public sealed class EnumerateSignsQuestion : Question
	{
		public IConcept Concept
		{ get; }

		public Boolean Recursive
		{ get; }

		public EnumerateSignsQuestion(IConcept concept, Boolean recursive, IEnumerable<IStatement> preconditions = null)
			: base(preconditions)
		{
			if (concept == null) throw new ArgumentNullException(nameof(concept));

			Concept = concept;
			Recursive = recursive;
		}
	}
}
