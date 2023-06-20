namespace AabSemantics.Extensions.WPF.ViewModels.Questions
{
	[QuestionDescriptor]
	public class HasSignQuestion : QuestionViewModel<Modules.Set.Questions.HasSignQuestion>
	{
		[PropertyDescriptor(true, "Questions.Parameters.Concept")]
		public IConcept Concept
		{ get; set; }

		[PropertyDescriptor(true, "Set\\Questions.Parameters.Sign")]
		public IConcept Sign
		{ get; set; }

		[PropertyDescriptor(false, "Questions.Parameters.Recursive")]
		public bool Recursive
		{ get; set; }

		public override Modules.Set.Questions.HasSignQuestion BuildQuestionImplementation()
		{
			return new Modules.Set.Questions.HasSignQuestion(Concept, Sign, Recursive);
		}
	}
}
