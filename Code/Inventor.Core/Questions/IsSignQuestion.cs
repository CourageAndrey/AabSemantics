namespace Inventor.Core.Questions
{
	[QuestionDescriptor]
	public sealed class IsSignQuestion : Question
	{
		[PropertyDescriptor(true, "QuestionNames.ParamConcept")]
		public Concept Concept
		{ get; set; }
	}
}
