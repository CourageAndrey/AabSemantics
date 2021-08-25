namespace Inventor.Client.ViewModels.Questions
{
	[QuestionDescriptor]
	public class EnumerateDescendantsQuestion : QuestionViewModel<Core.Questions.EnumerateDescendantsQuestion>
	{
		[PropertyDescriptor(true, "QuestionNames.ParamConcept")]
		public Core.IConcept Concept
		{ get; set; }

		public override Core.Questions.EnumerateDescendantsQuestion BuildQuestionImplementation()
		{
			return new Core.Questions.EnumerateDescendantsQuestion(Concept);
		}
	}
}
