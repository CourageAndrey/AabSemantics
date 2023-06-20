namespace Inventor.Semantics.WPF.ViewModels.Questions
{
	[QuestionDescriptor]
	public class IsSignQuestion : QuestionViewModel<Semantics.Set.Questions.IsSignQuestion>
	{
		[PropertyDescriptor(true, "Questions.Parameters.Concept")]
		public Semantics.IConcept Concept
		{ get; set; }

		public override Semantics.Set.Questions.IsSignQuestion BuildQuestionImplementation()
		{
			return new Semantics.Set.Questions.IsSignQuestion(Concept);
		}
	}
}
