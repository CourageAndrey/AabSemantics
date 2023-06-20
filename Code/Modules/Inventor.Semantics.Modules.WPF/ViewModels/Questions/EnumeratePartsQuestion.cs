namespace Inventor.Semantics.Modules.WPF.ViewModels.Questions
{
	[QuestionDescriptor]
	public class EnumeratePartsQuestion : QuestionViewModel<Semantics.Modules.Set.Questions.EnumeratePartsQuestion>
	{
		[PropertyDescriptor(true, "Questions.Parameters.Concept")]
		public Semantics.IConcept Concept
		{ get; set; }

		public override Semantics.Modules.Set.Questions.EnumeratePartsQuestion BuildQuestionImplementation()
		{
			return new Semantics.Modules.Set.Questions.EnumeratePartsQuestion(Concept);
		}
	}
}
