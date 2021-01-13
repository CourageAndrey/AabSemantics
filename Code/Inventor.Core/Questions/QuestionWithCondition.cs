﻿using System.Collections.Generic;

namespace Inventor.Core.Questions
{
	[QuestionDescriptor]
	public sealed class QuestionWithCondition : IQuestion
	{
		//[PropertyDescriptor(, )]
		public ICollection<IStatement> Conditions
		{ get; set; }

		//[PropertyDescriptor(, )]
		public IQuestion Question
		{ get; set; }
	}
}