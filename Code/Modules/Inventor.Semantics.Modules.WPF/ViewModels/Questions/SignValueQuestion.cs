namespace Inventor.Semantics.Modules.WPF.ViewModels.Questions
{
	[QuestionDescriptor]
	public class SignValueQuestion : QuestionViewModel<Set.Questions.SignValueQuestion>
	{
		[PropertyDescriptor(true, "Questions.Parameters.Concept")]
		public IConcept Concept
		{ get; set; }

		[PropertyDescriptor(true, "Set\\Questions.Parameters.Sign")]
		public IConcept Sign
		{ get; set; }

		public override Set.Questions.SignValueQuestion BuildQuestionImplementation()
		{
			return new Set.Questions.SignValueQuestion(Concept, Sign);
		}
	}
}
