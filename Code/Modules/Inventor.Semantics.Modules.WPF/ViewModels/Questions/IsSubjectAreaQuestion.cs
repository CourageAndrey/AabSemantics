namespace Inventor.Semantics.Modules.WPF.ViewModels.Questions
{
	[QuestionDescriptor]
	public class IsSubjectAreaQuestion : QuestionViewModel<Semantics.Modules.Set.Questions.IsSubjectAreaQuestion>
	{
		[PropertyDescriptor(true, "Questions.Parameters.Concept")]
		public Semantics.IConcept Concept
		{ get; set; }

		[PropertyDescriptor(true, "Set\\Questions.Parameters.Area")]
		public Semantics.IConcept Area
		{ get; set; }

		public override Semantics.Modules.Set.Questions.IsSubjectAreaQuestion BuildQuestionImplementation()
		{
			return new Semantics.Modules.Set.Questions.IsSubjectAreaQuestion(Concept, Area);
		}
	}
}
