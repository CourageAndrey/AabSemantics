namespace Inventor.Semantics.WPF.ViewModels.Questions
{
	[QuestionDescriptor]
	public class IsValueQuestion : QuestionViewModel<Semantics.Set.Questions.IsValueQuestion>
	{
		[PropertyDescriptor(true, "Questions.Parameters.Concept")]
		public Semantics.IConcept Concept
		{ get; set; }

		public override Semantics.Set.Questions.IsValueQuestion BuildQuestionImplementation()
		{
			return new Semantics.Set.Questions.IsValueQuestion(Concept);
		}
	}
}
