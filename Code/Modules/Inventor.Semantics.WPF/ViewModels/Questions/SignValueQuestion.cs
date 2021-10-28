namespace Inventor.WPF.ViewModels.Questions
{
	[QuestionDescriptor]
	public class SignValueQuestion : QuestionViewModel<Semantics.Set.Questions.SignValueQuestion>
	{
		[PropertyDescriptor(true, "Questions.Parameters.Concept")]
		public Semantics.IConcept Concept
		{ get; set; }

		[PropertyDescriptor(true, "Set\\Questions.Parameters.Sign")]
		public Semantics.IConcept Sign
		{ get; set; }

		public override Semantics.Set.Questions.SignValueQuestion BuildQuestionImplementation()
		{
			return new Semantics.Set.Questions.SignValueQuestion(Concept, Sign);
		}
	}
}
