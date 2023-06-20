namespace Inventor.Semantics.Modules.WPF.ViewModels.Questions
{
	[QuestionDescriptor]
	public class EnumerateContainersQuestion : QuestionViewModel<Semantics.Modules.Set.Questions.EnumerateContainersQuestion>
	{
		[PropertyDescriptor(true, "Questions.Parameters.Concept")]
		public Semantics.IConcept Concept
		{ get; set; }

		public override Semantics.Modules.Set.Questions.EnumerateContainersQuestion BuildQuestionImplementation()
		{
			return new Semantics.Modules.Set.Questions.EnumerateContainersQuestion(Concept);
		}
	}
}
