namespace Inventor.Semantics.Modules.WPF.ViewModels.Questions
{
	[QuestionDescriptor]
	public class FindSubjectAreaQuestion : QuestionViewModel<Set.Questions.FindSubjectAreaQuestion>
	{
		[PropertyDescriptor(true, "Questions.Parameters.Concept")]
		public IConcept Concept
		{ get; set; }

		public override Set.Questions.FindSubjectAreaQuestion BuildQuestionImplementation()
		{
			return new Set.Questions.FindSubjectAreaQuestion(Concept);
		}
	}
}
