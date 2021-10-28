namespace Inventor.WPF.ViewModels.Questions
{
	[QuestionDescriptor]
	public class EnumerateContainersQuestion : QuestionViewModel<Semantics.Set.Questions.EnumerateContainersQuestion>
	{
		[PropertyDescriptor(true, "Questions.Parameters.Concept")]
		public Semantics.IConcept Concept
		{ get; set; }

		public override Semantics.Set.Questions.EnumerateContainersQuestion BuildQuestionImplementation()
		{
			return new Semantics.Set.Questions.EnumerateContainersQuestion(Concept);
		}
	}
}
