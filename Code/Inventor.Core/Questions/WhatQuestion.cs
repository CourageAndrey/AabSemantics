namespace Inventor.Core.Questions
{
	[QuestionDescriptor]
	public sealed class WhatQuestion : Question
	{
		[PropertyDescriptor(true, "QuestionNames.ParamConcept")]
		public IConcept Concept
		{ get; set; }
	}
}
