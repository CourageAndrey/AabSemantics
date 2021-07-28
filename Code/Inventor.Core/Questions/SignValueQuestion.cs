using System;
using System.Collections.Generic;

using Inventor.Core.Statements;

namespace Inventor.Core.Questions
{
	public sealed class SignValueQuestion : Question, IQuestion<SignValueStatement>
	{
		public IConcept Concept
		{ get; }

		public IConcept Sign
		{ get; }

		public SignValueQuestion(IConcept concept, IConcept sign, IEnumerable<IStatement> preconditions = null)
			: base(preconditions)
		{
			if (concept == null) throw new ArgumentNullException(nameof(concept));
			if (sign == null) throw new ArgumentNullException(nameof(sign));

			Concept = concept;
			Sign = sign;
		}
	}
}
