namespace Inventor.Semantics.Extensions.WPF.ViewModels.Questions
{
	[QuestionDescriptor]
	public class IsSubjectAreaQuestion : QuestionViewModel<Modules.Set.Questions.IsSubjectAreaQuestion>
	{
		[PropertyDescriptor(true, "Questions.Parameters.Concept")]
		public IConcept Concept
		{ get; set; }

		[PropertyDescriptor(true, "Set\\Questions.Parameters.Area")]
		public IConcept Area
		{ get; set; }

		public override Modules.Set.Questions.IsSubjectAreaQuestion BuildQuestionImplementation()
		{
			return new Modules.Set.Questions.IsSubjectAreaQuestion(Concept, Area);
		}
	}
}
