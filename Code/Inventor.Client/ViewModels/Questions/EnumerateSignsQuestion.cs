namespace Inventor.Client.ViewModels.Questions
{
	[QuestionDescriptor]
	public class EnumerateSignsQuestion : QuestionViewModel<Core.Questions.EnumerateSignsQuestion>
	{
		[PropertyDescriptor(true, "Questions.Parameters.Concept")]
		public Core.IConcept Concept
		{ get; set; }

		[PropertyDescriptor(false, "Questions.Parameters.Recursive")]
		public bool Recursive
		{ get; set; }

		public override Core.Questions.EnumerateSignsQuestion BuildQuestionImplementation()
		{
			return new Core.Questions.EnumerateSignsQuestion(Concept, Recursive);
		}
	}
}
