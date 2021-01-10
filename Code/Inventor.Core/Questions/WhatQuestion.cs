namespace Inventor.Core.Questions
{
	[QuestionDescriptor]
	public sealed class WhatQuestion : IQuestion
	{
		[PropertyDescriptor(true, "QuestionNames.ParamConcept")]
		public IConcept Concept
		{ get; set; }
	}
}
