namespace Inventor.Semantics.Extensions.WPF.ViewModels.Questions
{
	[QuestionDescriptor]
	public class IsSignQuestion : QuestionViewModel<Modules.Set.Questions.IsSignQuestion>
	{
		[PropertyDescriptor(true, "Questions.Parameters.Concept")]
		public IConcept Concept
		{ get; set; }

		public override Modules.Set.Questions.IsSignQuestion BuildQuestionImplementation()
		{
			return new Modules.Set.Questions.IsSignQuestion(Concept);
		}
	}
}
