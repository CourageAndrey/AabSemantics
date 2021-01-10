using System;

namespace Inventor.Core.Questions
{
	[QuestionDescriptor]
	public sealed class HasSignQuestion : Question
	{
		[PropertyDescriptor(true, "QuestionNames.ParamConcept")]
		public IConcept Concept
		{ get; set; }

		[PropertyDescriptor(true, "QuestionNames.ParamSign")]
		public IConcept Sign
		{ get; set; }

		[PropertyDescriptor(false, "QuestionNames.ParamRecursive")]
		public Boolean Recursive
		{ get; set; }
	}
}
