namespace Inventor.WPF.ViewModels.Questions
{
	[QuestionDescriptor]
	public class HasSignQuestion : QuestionViewModel<Set.Questions.HasSignQuestion>
	{
		[PropertyDescriptor(true, "Questions.Parameters.Concept")]
		public Semantics.IConcept Concept
		{ get; set; }

		[PropertyDescriptor(true, "Set\\Questions.Parameters.Sign")]
		public Semantics.IConcept Sign
		{ get; set; }

		[PropertyDescriptor(false, "Questions.Parameters.Recursive")]
		public bool Recursive
		{ get; set; }

		public override Set.Questions.HasSignQuestion BuildQuestionImplementation()
		{
			return new Set.Questions.HasSignQuestion(Concept, Sign, Recursive);
		}
	}
}
