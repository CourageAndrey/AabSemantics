namespace Inventor.Core.Questions
{
	[QuestionDescriptor]
	public sealed class IsQuestion : Question
	{
		[PropertyDescriptor(true, "QuestionNames.ParamChild")]
		public Concept ChildConcept
		{ get; set; }

		[PropertyDescriptor(true, "QuestionNames.ParamParent")]
		public Concept ParentConcept
		{ get; set; }
	}
}
