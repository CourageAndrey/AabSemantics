namespace Inventor.Client.ViewModels.Questions
{
	[QuestionDescriptor]
	public class DescribeSubjectAreaQuestion : QuestionViewModel<Core.Questions.DescribeSubjectAreaQuestion>
	{
		[PropertyDescriptor(true, "Names.ParamConcept")]
		public Core.IConcept Concept
		{ get; set; }

		public override Core.Questions.DescribeSubjectAreaQuestion BuildQuestionImplementation()
		{
			return new Core.Questions.DescribeSubjectAreaQuestion(Concept);
		}
	}
}
