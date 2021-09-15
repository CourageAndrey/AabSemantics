namespace Inventor.Client.ViewModels.Questions
{
	[QuestionDescriptor]
	public class FindSubjectAreaQuestion : QuestionViewModel<Core.Questions.FindSubjectAreaQuestion>
	{
		[PropertyDescriptor(true, "Questions.Parameters.Concept")]
		public Core.IConcept Concept
		{ get; set; }

		public override Core.Questions.FindSubjectAreaQuestion BuildQuestionImplementation()
		{
			return new Core.Questions.FindSubjectAreaQuestion(Concept);
		}
	}
}
