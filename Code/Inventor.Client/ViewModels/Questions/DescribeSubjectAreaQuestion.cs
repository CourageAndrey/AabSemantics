namespace Inventor.Client.ViewModels.Questions
{
	[QuestionDescriptor]
	public sealed class DescribeSubjectAreaQuestion : QuestionViewModel<Core.Questions.DescribeSubjectAreaQuestion>
	{
		[PropertyDescriptor(true, "QuestionNames.ParamConcept")]
		public Core.IConcept Concept
		{ get; set; }

		public override Core.Questions.DescribeSubjectAreaQuestion BuildQuestion()
		{
			return new Core.Questions.DescribeSubjectAreaQuestion(Concept);
		}
	}
}
