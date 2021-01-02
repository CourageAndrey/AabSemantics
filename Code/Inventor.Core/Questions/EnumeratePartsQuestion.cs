namespace Inventor.Core.Questions
{
	[QuestionDescriptor]
	public sealed class EnumeratePartsQuestion : Question
	{
		[PropertyDescriptor(true, "QuestionNames.ParamConcept")]
		public Concept Concept
		{ get; set; }
	}
}
