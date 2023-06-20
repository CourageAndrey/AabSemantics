namespace Inventor.Semantics.Modules.WPF.ViewModels.Questions
{
	[QuestionDescriptor]
	public class IsValueQuestion : QuestionViewModel<Semantics.Modules.Set.Questions.IsValueQuestion>
	{
		[PropertyDescriptor(true, "Questions.Parameters.Concept")]
		public Semantics.IConcept Concept
		{ get; set; }

		public override Semantics.Modules.Set.Questions.IsValueQuestion BuildQuestionImplementation()
		{
			return new Semantics.Modules.Set.Questions.IsValueQuestion(Concept);
		}
	}
}
