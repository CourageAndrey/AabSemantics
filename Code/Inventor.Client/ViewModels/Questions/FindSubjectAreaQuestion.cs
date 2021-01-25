namespace Inventor.Client.ViewModels.Questions
{
	[QuestionDescriptor]
	public sealed class FindSubjectAreaQuestion : QuestionViewModel<Core.Questions.FindSubjectAreaQuestion>
	{
		[PropertyDescriptor(true, "QuestionNames.ParamConcept")]
		public Core.IConcept Concept
		{ get; set; }

		public override Core.Questions.FindSubjectAreaQuestion BuildQuestion()
		{
			return new Core.Questions.FindSubjectAreaQuestion(Concept);
		}
	}
}
