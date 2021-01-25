using System;

namespace Inventor.Core.Questions
{
	public sealed class IsQuestion : IQuestion
	{
		public IConcept Child
		{ get; }

		public IConcept Parent
		{ get; }

		public IsQuestion(IConcept child, IConcept parent)
		{
			if (child == null) throw new ArgumentNullException(nameof(child));
			if (parent == null) throw new ArgumentNullException(nameof(parent));

			Child = child;
			Parent = parent;
		}
	}
}
