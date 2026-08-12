namespace AabSemantics.Extensions.WPF.ViewModels.Questions
{
	public class FindSubjectAreaQuestion : QuestionViewModel<Modules.Set.Questions.FindSubjectAreaQuestion>
	{
		[PropertyDescriptor(true, "Questions.Parameters.Concept")]
		public IConcept Concept
		{ get; set; }

		public override Modules.Set.Questions.FindSubjectAreaQuestion BuildQuestionImplementation()
		{
			return new Modules.Set.Questions.FindSubjectAreaQuestion(Concept);
		}
	}
}
