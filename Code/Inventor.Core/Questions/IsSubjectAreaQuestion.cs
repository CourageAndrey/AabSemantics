namespace Inventor.Core.Questions
{
	[QuestionDescriptor]
	public sealed class IsSubjectAreaQuestion : IQuestion
	{
		[PropertyDescriptor(true, "QuestionNames.ParamConcept")]
		public IConcept Concept
		{ get; set; }

		[PropertyDescriptor(true, "QuestionNames.ParamArea")]
		public IConcept Area
		{ get; set; }
	}
}
