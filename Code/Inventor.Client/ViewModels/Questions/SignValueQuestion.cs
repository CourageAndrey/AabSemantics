namespace Inventor.Client.ViewModels.Questions
{
	[QuestionDescriptor]
	public class SignValueQuestion : QuestionViewModel<Core.Questions.SignValueQuestion>
	{
		[PropertyDescriptor(true, "Names.ParamConcept")]
		public Core.IConcept Concept
		{ get; set; }

		[PropertyDescriptor(true, "Names.ParamSign")]
		public Core.IConcept Sign
		{ get; set; }

		public override Core.Questions.SignValueQuestion BuildQuestionImplementation()
		{
			return new Core.Questions.SignValueQuestion(Concept, Sign);
		}
	}
}
