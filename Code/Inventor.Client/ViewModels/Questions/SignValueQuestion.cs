namespace Inventor.Client.ViewModels.Questions
{
	[QuestionDescriptor]
	public sealed class SignValueQuestion : QuestionViewModel<Core.Questions.SignValueQuestion>
	{
		[PropertyDescriptor(true, "QuestionNames.ParamConcept")]
		public Core.IConcept Concept
		{ get; set; }

		[PropertyDescriptor(true, "QuestionNames.ParamSign")]
		public Core.IConcept Sign
		{ get; set; }

		public override Core.Questions.SignValueQuestion BuildQuestionImplementation()
		{
			return new Core.Questions.SignValueQuestion(Concept, Sign);
		}
	}
}
