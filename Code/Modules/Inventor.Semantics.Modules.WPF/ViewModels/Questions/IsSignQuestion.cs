namespace Inventor.Semantics.Modules.WPF.ViewModels.Questions
{
	[QuestionDescriptor]
	public class IsSignQuestion : QuestionViewModel<Semantics.Modules.Set.Questions.IsSignQuestion>
	{
		[PropertyDescriptor(true, "Questions.Parameters.Concept")]
		public Semantics.IConcept Concept
		{ get; set; }

		public override Semantics.Modules.Set.Questions.IsSignQuestion BuildQuestionImplementation()
		{
			return new Semantics.Modules.Set.Questions.IsSignQuestion(Concept);
		}
	}
}
