namespace Inventor.WPF.ViewModels.Questions
{
	[QuestionDescriptor]
	public class FindSubjectAreaQuestion : QuestionViewModel<Set.Questions.FindSubjectAreaQuestion>
	{
		[PropertyDescriptor(true, "Questions.Parameters.Concept")]
		public Core.IConcept Concept
		{ get; set; }

		public override Set.Questions.FindSubjectAreaQuestion BuildQuestionImplementation()
		{
			return new Set.Questions.FindSubjectAreaQuestion(Concept);
		}
	}
}
