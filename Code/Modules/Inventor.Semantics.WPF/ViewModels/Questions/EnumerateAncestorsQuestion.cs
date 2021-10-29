namespace Inventor.Semantics.WPF.ViewModels.Questions
{
	[QuestionDescriptor]
	public class EnumerateAncestorsQuestion : QuestionViewModel<Modules.Classification.Questions.EnumerateAncestorsQuestion>
	{
		[PropertyDescriptor(true, "Questions.Parameters.Concept")]
		public Semantics.IConcept Concept
		{ get; set; }

		public override Modules.Classification.Questions.EnumerateAncestorsQuestion BuildQuestionImplementation()
		{
			return new Modules.Classification.Questions.EnumerateAncestorsQuestion(Concept);
		}
	}
}
