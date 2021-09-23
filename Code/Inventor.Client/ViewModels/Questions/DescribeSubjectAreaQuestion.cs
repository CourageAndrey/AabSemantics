namespace Inventor.Client.ViewModels.Questions
{
	[QuestionDescriptor]
	public class DescribeSubjectAreaQuestion : QuestionViewModel<Set.Questions.DescribeSubjectAreaQuestion>
	{
		[PropertyDescriptor(true, "Questions.Parameters.Concept")]
		public Core.IConcept Concept
		{ get; set; }

		public override Set.Questions.DescribeSubjectAreaQuestion BuildQuestionImplementation()
		{
			return new Set.Questions.DescribeSubjectAreaQuestion(Concept);
		}
	}
}
