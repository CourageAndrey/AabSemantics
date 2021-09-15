namespace Inventor.Client.ViewModels.Questions
{
	[QuestionDescriptor]
	public class SignValueQuestion : QuestionViewModel<Core.Questions.SignValueQuestion>
	{
		[PropertyDescriptor(true, "Questions.Parameters.Concept")]
		public Core.IConcept Concept
		{ get; set; }

		[PropertyDescriptor(true, "Set\\Questions.Parameters.Sign")]
		public Core.IConcept Sign
		{ get; set; }

		public override Core.Questions.SignValueQuestion BuildQuestionImplementation()
		{
			return new Core.Questions.SignValueQuestion(Concept, Sign);
		}
	}
}
