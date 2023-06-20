namespace Inventor.Semantics.Extensions.WPF.ViewModels.Questions
{
	[QuestionDescriptor]
	public class HasSignsQuestion : QuestionViewModel<Modules.Set.Questions.HasSignsQuestion>
	{
		[PropertyDescriptor(true, "Questions.Parameters.Concept")]
		public IConcept Concept
		{ get; set; }

		[PropertyDescriptor(false, "Questions.Parameters.Recursive")]
		public bool Recursive
		{ get; set; }

		public override Modules.Set.Questions.HasSignsQuestion BuildQuestionImplementation()
		{
			return new Modules.Set.Questions.HasSignsQuestion(Concept, Recursive);
		}
	}
}
