namespace AabSemantics.Extensions.WPF.ViewModels.Questions
{
	[QuestionDescriptor]
	public class EnumeratePartsQuestion : QuestionViewModel<Modules.Set.Questions.EnumeratePartsQuestion>
	{
		[PropertyDescriptor(true, "Questions.Parameters.Concept")]
		public IConcept Concept
		{ get; set; }

		public override Modules.Set.Questions.EnumeratePartsQuestion BuildQuestionImplementation()
		{
			return new Modules.Set.Questions.EnumeratePartsQuestion(Concept);
		}
	}
}
