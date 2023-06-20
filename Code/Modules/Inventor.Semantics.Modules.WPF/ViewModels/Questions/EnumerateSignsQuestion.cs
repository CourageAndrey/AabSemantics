namespace Inventor.Semantics.Modules.WPF.ViewModels.Questions
{
	[QuestionDescriptor]
	public class EnumerateSignsQuestion : QuestionViewModel<Semantics.Modules.Set.Questions.EnumerateSignsQuestion>
	{
		[PropertyDescriptor(true, "Questions.Parameters.Concept")]
		public Semantics.IConcept Concept
		{ get; set; }

		[PropertyDescriptor(false, "Questions.Parameters.Recursive")]
		public bool Recursive
		{ get; set; }

		public override Semantics.Modules.Set.Questions.EnumerateSignsQuestion BuildQuestionImplementation()
		{
			return new Semantics.Modules.Set.Questions.EnumerateSignsQuestion(Concept, Recursive);
		}
	}
}
