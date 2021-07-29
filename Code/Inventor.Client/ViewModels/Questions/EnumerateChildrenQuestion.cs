namespace Inventor.Client.ViewModels.Questions
{
	[QuestionDescriptor]
	public class EnumerateChildrenQuestion : QuestionViewModel<Core.Questions.EnumerateChildrenQuestion>
	{
		[PropertyDescriptor(true, "QuestionNames.ParamConcept")]
		public Core.IConcept Concept
		{ get; set; }

		public override Core.Questions.EnumerateChildrenQuestion BuildQuestionImplementation()
		{
			return new Core.Questions.EnumerateChildrenQuestion(Concept);
		}
	}
}
