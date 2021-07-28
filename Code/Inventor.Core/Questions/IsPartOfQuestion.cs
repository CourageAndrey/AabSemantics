using System;

namespace Inventor.Core.Questions
{
	public sealed class IsPartOfQuestion : Question
	{
		public IConcept Parent
		{ get; }

		public IConcept Child
		{ get; }

		public IsPartOfQuestion(IConcept child, IConcept parent)
		{
			if (child == null) throw new ArgumentNullException(nameof(child));
			if (parent == null) throw new ArgumentNullException(nameof(parent));

			Child = child;
			Parent = parent;
		}
	}
}
