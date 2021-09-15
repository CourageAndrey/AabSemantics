namespace Inventor.Client.ViewModels.Questions
{
	[QuestionDescriptor]
	public class IsPartOfQuestion : QuestionViewModel<Core.Questions.IsPartOfQuestion>
	{
		[PropertyDescriptor(true, "Questions.Parameters.Parent")]
		public Core.IConcept Parent
		{ get; set; }

		[PropertyDescriptor(true, "Questions.Parameters.Child")]
		public Core.IConcept Child
		{ get; set; }

		public override Core.Questions.IsPartOfQuestion BuildQuestionImplementation()
		{
			return new Core.Questions.IsPartOfQuestion(Child, Parent);
		}
	}
}
