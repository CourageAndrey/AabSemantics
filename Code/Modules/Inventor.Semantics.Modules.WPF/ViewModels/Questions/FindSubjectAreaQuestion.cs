namespace Inventor.Semantics.Modules.WPF.ViewModels.Questions
{
	[QuestionDescriptor]
	public class FindSubjectAreaQuestion : QuestionViewModel<Semantics.Modules.Set.Questions.FindSubjectAreaQuestion>
	{
		[PropertyDescriptor(true, "Questions.Parameters.Concept")]
		public Semantics.IConcept Concept
		{ get; set; }

		public override Semantics.Modules.Set.Questions.FindSubjectAreaQuestion BuildQuestionImplementation()
		{
			return new Semantics.Modules.Set.Questions.FindSubjectAreaQuestion(Concept);
		}
	}
}
