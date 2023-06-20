namespace AabSemantics.Extensions.WPF.ViewModels.Questions
{
	[QuestionDescriptor]
	public class EnumerateDescendantsQuestion : QuestionViewModel<Modules.Classification.Questions.EnumerateDescendantsQuestion>
	{
		[PropertyDescriptor(true, "Questions.Parameters.Concept")]
		public IConcept Concept
		{ get; set; }

		public override Modules.Classification.Questions.EnumerateDescendantsQuestion BuildQuestionImplementation()
		{
			return new Modules.Classification.Questions.EnumerateDescendantsQuestion(Concept);
		}
	}
}
