namespace Inventor.Core.Questions
{
	[QuestionDescriptor]
	public sealed class DescribeSubjectAreaQuestion : IQuestion
	{
		[PropertyDescriptor(true, "QuestionNames.ParamConcept")]
		public IConcept Concept
		{ get; set; }
	}
}
