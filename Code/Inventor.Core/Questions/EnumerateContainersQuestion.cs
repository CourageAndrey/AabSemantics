namespace Inventor.Core.Questions
{
	[QuestionDescriptor]
	public sealed class EnumerateContainersQuestion : Question
	{
		[PropertyDescriptor(true, "QuestionNames.ParamConcept")]
		public IConcept Concept
		{ get; set; }
	}
}
