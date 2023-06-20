namespace Inventor.Semantics.Modules.WPF.ViewModels.Questions
{
	[QuestionDescriptor]
	public class IsSignQuestion : QuestionViewModel<Set.Questions.IsSignQuestion>
	{
		[PropertyDescriptor(true, "Questions.Parameters.Concept")]
		public IConcept Concept
		{ get; set; }

		public override Set.Questions.IsSignQuestion BuildQuestionImplementation()
		{
			return new Set.Questions.IsSignQuestion(Concept);
		}
	}
}
