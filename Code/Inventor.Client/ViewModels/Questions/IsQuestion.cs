namespace Inventor.Client.ViewModels.Questions
{
	[QuestionDescriptor]
	public class IsQuestion : QuestionViewModel<Core.Questions.IsQuestion>
	{
		[PropertyDescriptor(true, "Questions.Parameters.Child")]
		public Core.IConcept Child
		{ get; set; }

		[PropertyDescriptor(true, "Questions.Parameters.Parent")]
		public Core.IConcept Parent
		{ get; set; }

		public override Core.Questions.IsQuestion BuildQuestionImplementation()
		{
			return new Core.Questions.IsQuestion(Child, Parent);
		}
	}
}
