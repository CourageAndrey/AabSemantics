namespace Inventor.Semantics.Modules.WPF.ViewModels.Questions
{
	[QuestionDescriptor]
	public class EnumerateContainersQuestion : QuestionViewModel<Set.Questions.EnumerateContainersQuestion>
	{
		[PropertyDescriptor(true, "Questions.Parameters.Concept")]
		public IConcept Concept
		{ get; set; }

		public override Set.Questions.EnumerateContainersQuestion BuildQuestionImplementation()
		{
			return new Set.Questions.EnumerateContainersQuestion(Concept);
		}
	}
}
