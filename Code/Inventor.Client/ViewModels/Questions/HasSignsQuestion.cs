namespace Inventor.Client.ViewModels.Questions
{
	[QuestionDescriptor]
	public class HasSignsQuestion : QuestionViewModel<Core.Questions.HasSignsQuestion>
	{
		[PropertyDescriptor(true, "Questions.Parameters.Concept")]
		public Core.IConcept Concept
		{ get; set; }

		[PropertyDescriptor(false, "Questions.Parameters.Recursive")]
		public bool Recursive
		{ get; set; }

		public override Core.Questions.HasSignsQuestion BuildQuestionImplementation()
		{
			return new Core.Questions.HasSignsQuestion(Concept, Recursive);
		}
	}
}
