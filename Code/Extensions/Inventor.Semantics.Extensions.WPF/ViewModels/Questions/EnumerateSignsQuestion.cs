namespace Inventor.Semantics.Extensions.WPF.ViewModels.Questions
{
	[QuestionDescriptor]
	public class EnumerateSignsQuestion : QuestionViewModel<Modules.Set.Questions.EnumerateSignsQuestion>
	{
		[PropertyDescriptor(true, "Questions.Parameters.Concept")]
		public IConcept Concept
		{ get; set; }

		[PropertyDescriptor(false, "Questions.Parameters.Recursive")]
		public bool Recursive
		{ get; set; }

		public override Modules.Set.Questions.EnumerateSignsQuestion BuildQuestionImplementation()
		{
			return new Modules.Set.Questions.EnumerateSignsQuestion(Concept, Recursive);
		}
	}
}
