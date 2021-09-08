namespace Inventor.Client.ViewModels.Questions
{
	[QuestionDescriptor]
	public class EnumerateContainersQuestion : QuestionViewModel<Core.Questions.EnumerateContainersQuestion>
	{
		[PropertyDescriptor(true, "Names.ParamConcept")]
		public Core.IConcept Concept
		{ get; set; }

		public override Core.Questions.EnumerateContainersQuestion BuildQuestionImplementation()
		{
			return new Core.Questions.EnumerateContainersQuestion(Concept);
		}
	}
}
