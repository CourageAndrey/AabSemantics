namespace Inventor.Semantics.WPF.ViewModels.Questions
{
	[QuestionDescriptor]
	public class FindSubjectAreaQuestion : QuestionViewModel<Semantics.Set.Questions.FindSubjectAreaQuestion>
	{
		[PropertyDescriptor(true, "Questions.Parameters.Concept")]
		public Semantics.IConcept Concept
		{ get; set; }

		public override Semantics.Set.Questions.FindSubjectAreaQuestion BuildQuestionImplementation()
		{
			return new Semantics.Set.Questions.FindSubjectAreaQuestion(Concept);
		}
	}
}
