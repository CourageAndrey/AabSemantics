using System;

namespace Inventor.Core.Questions
{
	[QuestionDescriptor]
	public sealed class HasSignsQuestion : Question
	{
		[PropertyDescriptor(true, "QuestionNames.ParamConcept")]
		public Concept Concept
		{ get; set; }

		[PropertyDescriptor(false, "QuestionNames.ParamRecursive")]
		public Boolean Recursive
		{ get; set; }
	}
}
