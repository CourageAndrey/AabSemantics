using System;
using System.Collections.Generic;

using Inventor.Core.Statements;

namespace Inventor.Core.Questions
{
	public sealed class IsPartOfQuestion : Question, IQuestion<HasPartStatement>
	{
		public IConcept Parent
		{ get; }

		public IConcept Child
		{ get; }

		public IsPartOfQuestion(IConcept child, IConcept parent, IEnumerable<IStatement> preconditions = null)
			: base(preconditions)
		{
			if (child == null) throw new ArgumentNullException(nameof(child));
			if (parent == null) throw new ArgumentNullException(nameof(parent));

			Child = child;
			Parent = parent;
		}
	}
}
