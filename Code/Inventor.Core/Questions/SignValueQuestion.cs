namespace Inventor.Core.Questions
{
	[QuestionDescriptor]
	public sealed class SignValueQuestion : Question
	{
		[PropertyDescriptor(true, "QuestionNames.ParamConcept")]
		public IConcept Concept
		{ get; set; }

		[PropertyDescriptor(true, "QuestionNames.ParamSign")]
		public IConcept Sign
		{ get; set; }
	}
}
