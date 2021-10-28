namespace Inventor.WPF.ViewModels.Questions
{
	[QuestionDescriptor]
	public class IsSignQuestion : QuestionViewModel<Set.Questions.IsSignQuestion>
	{
		[PropertyDescriptor(true, "Questions.Parameters.Concept")]
		public Semantics.IConcept Concept
		{ get; set; }

		public override Set.Questions.IsSignQuestion BuildQuestionImplementation()
		{
			return new Set.Questions.IsSignQuestion(Concept);
		}
	}
}
