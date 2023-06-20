namespace AabSemantics.Extensions.WPF.ViewModels.Questions
{
	[QuestionDescriptor]
	public class SignValueQuestion : QuestionViewModel<Modules.Set.Questions.SignValueQuestion>
	{
		[PropertyDescriptor(true, "Questions.Parameters.Concept")]
		public IConcept Concept
		{ get; set; }

		[PropertyDescriptor(true, "Set\\Questions.Parameters.Sign")]
		public IConcept Sign
		{ get; set; }

		public override Modules.Set.Questions.SignValueQuestion BuildQuestionImplementation()
		{
			return new Modules.Set.Questions.SignValueQuestion(Concept, Sign);
		}
	}
}
