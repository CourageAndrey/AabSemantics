namespace Inventor.Core.Questions
{
	[QuestionDescriptor]
	public sealed class FindSubjectAreaQuestion : IQuestion
	{
		[PropertyDescriptor(true, "QuestionNames.ParamConcept")]
		public IConcept Concept
		{ get; set; }
	}
}
