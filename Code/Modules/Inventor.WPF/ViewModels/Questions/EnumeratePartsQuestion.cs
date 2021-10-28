namespace Inventor.WPF.ViewModels.Questions
{
	[QuestionDescriptor]
	public class EnumeratePartsQuestion : QuestionViewModel<Set.Questions.EnumeratePartsQuestion>
	{
		[PropertyDescriptor(true, "Questions.Parameters.Concept")]
		public Semantics.IConcept Concept
		{ get; set; }

		public override Set.Questions.EnumeratePartsQuestion BuildQuestionImplementation()
		{
			return new Set.Questions.EnumeratePartsQuestion(Concept);
		}
	}
}
