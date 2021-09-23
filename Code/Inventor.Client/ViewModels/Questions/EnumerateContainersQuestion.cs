namespace Inventor.Client.ViewModels.Questions
{
	[QuestionDescriptor]
	public class EnumerateContainersQuestion : QuestionViewModel<Set.Questions.EnumerateContainersQuestion>
	{
		[PropertyDescriptor(true, "Questions.Parameters.Concept")]
		public Core.IConcept Concept
		{ get; set; }

		public override Set.Questions.EnumerateContainersQuestion BuildQuestionImplementation()
		{
			return new Set.Questions.EnumerateContainersQuestion(Concept);
		}
	}
}
