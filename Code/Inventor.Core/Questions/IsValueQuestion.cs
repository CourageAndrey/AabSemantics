using System;
using System.Collections.Generic;

using Inventor.Core.Statements;

namespace Inventor.Core.Questions
{
	public sealed class IsValueQuestion : Question, IQuestion<SignValueStatement>
	{
		public IConcept Concept
		{ get; }

		public IsValueQuestion(IConcept concept, IEnumerable<IStatement> preconditions = null)
			: base(preconditions)
		{
			if (concept == null) throw new ArgumentNullException(nameof(concept));

			Concept = concept;
		}
	}
}
