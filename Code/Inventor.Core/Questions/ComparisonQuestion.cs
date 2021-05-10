using System;

namespace Inventor.Core.Questions
{
	public sealed class ComparisonQuestion : IQuestion
	{
		public IConcept LeftValue
		{ get; set; }

		public IConcept RightValue
		{ get; set; }

		public ComparisonQuestion(IConcept leftValue, IConcept rightValue)
		{
			if (leftValue == null) throw new ArgumentNullException(nameof(leftValue));
			if (rightValue == null) throw new ArgumentNullException(nameof(rightValue));

			LeftValue = leftValue;
			RightValue = rightValue;
		}
	}
}
