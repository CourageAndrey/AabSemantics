namespace Inventor.Semantics.Extensions.WPF.ViewModels.Questions
{
	[QuestionDescriptor]
	public class EnumerateContainersQuestion : QuestionViewModel<Modules.Set.Questions.EnumerateContainersQuestion>
	{
		[PropertyDescriptor(true, "Questions.Parameters.Concept")]
		public IConcept Concept
		{ get; set; }

		public override Modules.Set.Questions.EnumerateContainersQuestion BuildQuestionImplementation()
		{
			return new Modules.Set.Questions.EnumerateContainersQuestion(Concept);
		}
	}
}
