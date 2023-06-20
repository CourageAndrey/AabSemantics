namespace Inventor.Semantics.Modules.WPF.ViewModels.Questions
{
	[QuestionDescriptor]
	public class HasSignsQuestion : QuestionViewModel<Semantics.Modules.Set.Questions.HasSignsQuestion>
	{
		[PropertyDescriptor(true, "Questions.Parameters.Concept")]
		public Semantics.IConcept Concept
		{ get; set; }

		[PropertyDescriptor(false, "Questions.Parameters.Recursive")]
		public bool Recursive
		{ get; set; }

		public override Semantics.Modules.Set.Questions.HasSignsQuestion BuildQuestionImplementation()
		{
			return new Semantics.Modules.Set.Questions.HasSignsQuestion(Concept, Recursive);
		}
	}
}
