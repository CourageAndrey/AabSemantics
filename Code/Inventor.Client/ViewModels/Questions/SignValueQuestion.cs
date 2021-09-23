namespace Inventor.Client.ViewModels.Questions
{
	[QuestionDescriptor]
	public class SignValueQuestion : QuestionViewModel<Set.Questions.SignValueQuestion>
	{
		[PropertyDescriptor(true, "Questions.Parameters.Concept")]
		public Core.IConcept Concept
		{ get; set; }

		[PropertyDescriptor(true, "Set\\Questions.Parameters.Sign")]
		public Core.IConcept Sign
		{ get; set; }

		public override Set.Questions.SignValueQuestion BuildQuestionImplementation()
		{
			return new Set.Questions.SignValueQuestion(Concept, Sign);
		}
	}
}
