using System;
using System.Collections.Generic;

namespace Inventor.Core.Questions
{
	public sealed class IsQuestion : Question
	{
		public IConcept Child
		{ get; }

		public IConcept Parent
		{ get; }

		public IsQuestion(IConcept child, IConcept parent, IEnumerable<IStatement> preconditions = null)
			: base(preconditions)
		{
			if (child == null) throw new ArgumentNullException(nameof(child));
			if (parent == null) throw new ArgumentNullException(nameof(parent));

			Child = child;
			Parent = parent;
		}
	}
}
