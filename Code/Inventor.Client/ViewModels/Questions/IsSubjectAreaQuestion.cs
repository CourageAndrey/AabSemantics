namespace Inventor.Client.ViewModels.Questions
{
	[QuestionDescriptor]
	public class IsSubjectAreaQuestion : QuestionViewModel<Core.Questions.IsSubjectAreaQuestion>
	{
		[PropertyDescriptor(true, "Questions.Parameters.Concept")]
		public Core.IConcept Concept
		{ get; set; }

		[PropertyDescriptor(true, "Set\\Questions.Parameters.Area")]
		public Core.IConcept Area
		{ get; set; }

		public override Core.Questions.IsSubjectAreaQuestion BuildQuestionImplementation()
		{
			return new Core.Questions.IsSubjectAreaQuestion(Concept, Area);
		}
	}
}
