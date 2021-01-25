namespace Inventor.Client.ViewModels.Questions
{
	[QuestionDescriptor]
	public sealed class IsSubjectAreaQuestion : QuestionViewModel<Core.Questions.IsSubjectAreaQuestion>
	{
		[PropertyDescriptor(true, "QuestionNames.ParamConcept")]
		public Core.IConcept Concept
		{ get; set; }

		[PropertyDescriptor(true, "QuestionNames.ParamArea")]
		public Core.IConcept Area
		{ get; set; }

		public override Core.Questions.IsSubjectAreaQuestion BuildQuestion()
		{
			return new Core.Questions.IsSubjectAreaQuestion(Concept, Area);
		}
	}
}
