namespace Inventor.Core.Questions
{
	[QuestionDescriptor]
	public sealed class IsSubjectAreaQuestion : Question
	{
		[PropertyDescriptor(true, "QuestionNames.ParamConcept")]
		public IConcept Concept
		{ get; set; }

		[PropertyDescriptor(true, "QuestionNames.ParamArea")]
		public IConcept Area
		{ get; set; }
	}
}
