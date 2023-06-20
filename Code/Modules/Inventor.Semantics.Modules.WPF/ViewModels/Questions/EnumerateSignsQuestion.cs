namespace Inventor.Semantics.Modules.WPF.ViewModels.Questions
{
	[QuestionDescriptor]
	public class EnumerateSignsQuestion : QuestionViewModel<Set.Questions.EnumerateSignsQuestion>
	{
		[PropertyDescriptor(true, "Questions.Parameters.Concept")]
		public IConcept Concept
		{ get; set; }

		[PropertyDescriptor(false, "Questions.Parameters.Recursive")]
		public bool Recursive
		{ get; set; }

		public override Set.Questions.EnumerateSignsQuestion BuildQuestionImplementation()
		{
			return new Set.Questions.EnumerateSignsQuestion(Concept, Recursive);
		}
	}
}
