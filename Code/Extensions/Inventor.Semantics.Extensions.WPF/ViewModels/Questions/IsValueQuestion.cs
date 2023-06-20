namespace Inventor.Semantics.Extensions.WPF.ViewModels.Questions
{
	[QuestionDescriptor]
	public class IsValueQuestion : QuestionViewModel<Modules.Set.Questions.IsValueQuestion>
	{
		[PropertyDescriptor(true, "Questions.Parameters.Concept")]
		public IConcept Concept
		{ get; set; }

		public override Modules.Set.Questions.IsValueQuestion BuildQuestionImplementation()
		{
			return new Modules.Set.Questions.IsValueQuestion(Concept);
		}
	}
}
