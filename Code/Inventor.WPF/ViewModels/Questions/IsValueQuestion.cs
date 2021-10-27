namespace Inventor.WPF.ViewModels.Questions
{
	[QuestionDescriptor]
	public class IsValueQuestion : QuestionViewModel<Set.Questions.IsValueQuestion>
	{
		[PropertyDescriptor(true, "Questions.Parameters.Concept")]
		public Semantics.IConcept Concept
		{ get; set; }

		public override Set.Questions.IsValueQuestion BuildQuestionImplementation()
		{
			return new Set.Questions.IsValueQuestion(Concept);
		}
	}
}
