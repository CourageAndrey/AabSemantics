namespace Inventor.Semantics.WPF.ViewModels.Questions
{
	[QuestionDescriptor]
	public class EnumeratePartsQuestion : QuestionViewModel<Semantics.Set.Questions.EnumeratePartsQuestion>
	{
		[PropertyDescriptor(true, "Questions.Parameters.Concept")]
		public Semantics.IConcept Concept
		{ get; set; }

		public override Semantics.Set.Questions.EnumeratePartsQuestion BuildQuestionImplementation()
		{
			return new Semantics.Set.Questions.EnumeratePartsQuestion(Concept);
		}
	}
}
