﻿using System;
using System.Collections.Generic;

using Inventor.Core.Statements;

namespace Inventor.Core.Questions
{
	public sealed class ComparisonQuestion : Question, IQuestion<ComparisonStatement>
	{
		public IConcept LeftValue
		{ get; set; }

		public IConcept RightValue
		{ get; set; }

		public ComparisonQuestion(IConcept leftValue, IConcept rightValue, IEnumerable<IStatement> preconditions = null)
			: base(preconditions)
		{
			if (leftValue == null) throw new ArgumentNullException(nameof(leftValue));
			if (rightValue == null) throw new ArgumentNullException(nameof(rightValue));

			LeftValue = leftValue;
			RightValue = rightValue;
		}
	}
}
