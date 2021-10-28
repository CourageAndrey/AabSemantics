namespace Inventor.WPF.ViewModels.Questions
{
	[QuestionDescriptor]
	public class EnumerateAncestorsQuestion : QuestionViewModel<Semantics.Questions.EnumerateAncestorsQuestion>
	{
		[PropertyDescriptor(true, "Questions.Parameters.Concept")]
		public Semantics.IConcept Concept
		{ get; set; }

		public override Semantics.Questions.EnumerateAncestorsQuestion BuildQuestionImplementation()
		{
			return new Semantics.Questions.EnumerateAncestorsQuestion(Concept);
		}
	}
}
