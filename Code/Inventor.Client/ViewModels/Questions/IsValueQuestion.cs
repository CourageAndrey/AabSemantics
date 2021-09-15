namespace Inventor.Client.ViewModels.Questions
{
	[QuestionDescriptor]
	public class IsValueQuestion : QuestionViewModel<Core.Questions.IsValueQuestion>
	{
		[PropertyDescriptor(true, "Questions.Parameters.Concept")]
		public Core.IConcept Concept
		{ get; set; }

		public override Core.Questions.IsValueQuestion BuildQuestionImplementation()
		{
			return new Core.Questions.IsValueQuestion(Concept);
		}
	}
}
