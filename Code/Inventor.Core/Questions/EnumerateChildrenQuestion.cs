namespace Inventor.Core.Questions
{
	[QuestionDescriptor]
	public sealed class EnumerateChildrenQuestion : Question
	{
		[PropertyDescriptor(true, "QuestionNames.ParamConcept")]
		public Concept Concept
		{ get; set; }
	}
}
