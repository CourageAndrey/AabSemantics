namespace Inventor.Semantics.Modules.WPF.ViewModels.Questions
{
	[QuestionDescriptor]
	public class EnumerateAncestorsQuestion : QuestionViewModel<Classification.Questions.EnumerateAncestorsQuestion>
	{
		[PropertyDescriptor(true, "Questions.Parameters.Concept")]
		public IConcept Concept
		{ get; set; }

		public override Classification.Questions.EnumerateAncestorsQuestion BuildQuestionImplementation()
		{
			return new Classification.Questions.EnumerateAncestorsQuestion(Concept);
		}
	}
}
