namespace Inventor.WPF.ViewModels.Questions
{
	[QuestionDescriptor]
	public class EnumerateSignsQuestion : QuestionViewModel<Semantics.Set.Questions.EnumerateSignsQuestion>
	{
		[PropertyDescriptor(true, "Questions.Parameters.Concept")]
		public Semantics.IConcept Concept
		{ get; set; }

		[PropertyDescriptor(false, "Questions.Parameters.Recursive")]
		public bool Recursive
		{ get; set; }

		public override Semantics.Set.Questions.EnumerateSignsQuestion BuildQuestionImplementation()
		{
			return new Semantics.Set.Questions.EnumerateSignsQuestion(Concept, Recursive);
		}
	}
}
