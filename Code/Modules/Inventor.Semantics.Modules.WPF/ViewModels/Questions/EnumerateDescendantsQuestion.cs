namespace Inventor.Semantics.Modules.WPF.ViewModels.Questions
{
	[QuestionDescriptor]
	public class EnumerateDescendantsQuestion : QuestionViewModel<Classification.Questions.EnumerateDescendantsQuestion>
	{
		[PropertyDescriptor(true, "Questions.Parameters.Concept")]
		public IConcept Concept
		{ get; set; }

		public override Classification.Questions.EnumerateDescendantsQuestion BuildQuestionImplementation()
		{
			return new Classification.Questions.EnumerateDescendantsQuestion(Concept);
		}
	}
}
