namespace Inventor.WPF.ViewModels.Questions
{
	[QuestionDescriptor]
	public class EnumerateDescendantsQuestion : QuestionViewModel<Semantics.Questions.EnumerateDescendantsQuestion>
	{
		[PropertyDescriptor(true, "Questions.Parameters.Concept")]
		public Semantics.IConcept Concept
		{ get; set; }

		public override Semantics.Questions.EnumerateDescendantsQuestion BuildQuestionImplementation()
		{
			return new Semantics.Questions.EnumerateDescendantsQuestion(Concept);
		}
	}
}
