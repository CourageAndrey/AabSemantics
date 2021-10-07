namespace Inventor.WPF.ViewModels.Questions
{
	[QuestionDescriptor]
	public class HasSignsQuestion : QuestionViewModel<Set.Questions.HasSignsQuestion>
	{
		[PropertyDescriptor(true, "Questions.Parameters.Concept")]
		public Core.IConcept Concept
		{ get; set; }

		[PropertyDescriptor(false, "Questions.Parameters.Recursive")]
		public bool Recursive
		{ get; set; }

		public override Set.Questions.HasSignsQuestion BuildQuestionImplementation()
		{
			return new Set.Questions.HasSignsQuestion(Concept, Recursive);
		}
	}
}
