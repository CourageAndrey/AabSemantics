namespace Inventor.Client.ViewModels.Questions
{
	[QuestionDescriptor]
	public sealed class EnumerateContainersQuestion : QuestionViewModel<Core.Questions.EnumerateContainersQuestion>
	{
		[PropertyDescriptor(true, "QuestionNames.ParamConcept")]
		public Core.IConcept Concept
		{ get; set; }

		public override Core.Questions.EnumerateContainersQuestion BuildQuestionImplementation()
		{
			return new Core.Questions.EnumerateContainersQuestion(Concept);
		}
	}
}
