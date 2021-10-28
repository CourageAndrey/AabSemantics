namespace Inventor.WPF.ViewModels.Questions
{
	[QuestionDescriptor]
	public class IsSubjectAreaQuestion : QuestionViewModel<Semantics.Set.Questions.IsSubjectAreaQuestion>
	{
		[PropertyDescriptor(true, "Questions.Parameters.Concept")]
		public Semantics.IConcept Concept
		{ get; set; }

		[PropertyDescriptor(true, "Set\\Questions.Parameters.Area")]
		public Semantics.IConcept Area
		{ get; set; }

		public override Semantics.Set.Questions.IsSubjectAreaQuestion BuildQuestionImplementation()
		{
			return new Semantics.Set.Questions.IsSubjectAreaQuestion(Concept, Area);
		}
	}
}
