namespace Inventor.Semantics.Modules.WPF.ViewModels.Questions
{
	[QuestionDescriptor]
	public class SignValueQuestion : QuestionViewModel<Semantics.Modules.Set.Questions.SignValueQuestion>
	{
		[PropertyDescriptor(true, "Questions.Parameters.Concept")]
		public Semantics.IConcept Concept
		{ get; set; }

		[PropertyDescriptor(true, "Set\\Questions.Parameters.Sign")]
		public Semantics.IConcept Sign
		{ get; set; }

		public override Semantics.Modules.Set.Questions.SignValueQuestion BuildQuestionImplementation()
		{
			return new Semantics.Modules.Set.Questions.SignValueQuestion(Concept, Sign);
		}
	}
}
