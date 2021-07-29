namespace Inventor.Client.ViewModels.Questions
{
	[QuestionDescriptor]
	public class IsSubjectAreaQuestion : QuestionViewModel<Core.Questions.IsSubjectAreaQuestion>
	{
		[PropertyDescriptor(true, "QuestionNames.ParamConcept")]
		public Core.IConcept Concept
		{ get; set; }

		[PropertyDescriptor(true, "QuestionNames.ParamArea")]
		public Core.IConcept Area
		{ get; set; }

		public override Core.Questions.IsSubjectAreaQuestion BuildQuestionImplementation()
		{
			return new Core.Questions.IsSubjectAreaQuestion(Concept, Area);
		}
	}
}
